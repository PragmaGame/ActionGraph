using System.Collections.Generic;
using System.Linq;
using Game.Core.ActionGraph.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    public class ActionGraphView : GraphView
    {
        private ActionGraphViewConfig _config;
        
        private MiniMap _miniMap;

        private Dictionary<string, List<ActionNode>> _keysMap;

        private KeyValidator _validator;

        public ActionGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddMiniMap();
            AddStyles();

            //SetElementsDeletedCallback();
            SetGraphViewChangedCallback();
            
            _config = (ActionGraphViewConfig)EditorGUIUtility.Load("NovelGraph/NovelGraphViewConfig.asset");

            _keysMap = new Dictionary<string, List<ActionNode>>();

            _validator = new KeyValidator(_config.KeyValidatorParam);
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextMenu());
            this.AddManipulator(CreateGroupContextMenu());
        }
        
        private void AddMiniMap()
        {
            _miniMap = new MiniMap()
            {
                anchored = true
            };

            _miniMap.SetPosition(new Rect(15, 50, 200, 180));

            Add(_miniMap);

            _miniMap.visible = false;
        }
        
        private IManipulator CreateNodeContextMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.InsertAction(0,"Create Node", 
                    action => CreateNode(GetGraphMousePosition(action.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }
        
        private IManipulator CreateGroupContextMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.InsertAction(1,"Create Group", 
                    action => CreateGroup(GetGraphMousePosition(action.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }
        
        public void LoadSnapshotData(GraphSnapshotData graphSnapshotData)
        {
            DeleteAll();
            
            var actionNodes = new Dictionary<string, ActionNode>();

            foreach (var nodeData in graphSnapshotData.nodes)
            {
                var node = CreateNode(nodeData.position, nodeData.key, nodeData.transitions, nodeData.metaData);
                
                actionNodes.Add(node.Key, node);
            }

            foreach (var graphNode in actionNodes)
            {
                foreach (Port transitionPort in graphNode.Value.outputContainer.Children())
                {
                    var transitionData = (TransitionData)transitionPort.userData;

                    if (string.IsNullOrEmpty(transitionData.nodeKey))
                    {
                        continue;
                    }

                    var nextNode = actionNodes[transitionData.nodeKey];

                    Port nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();

                    Edge edge = transitionPort.ConnectTo(nextNodeInputPort);

                    AddElement(edge);

                    graphNode.Value.RefreshPorts();
                }
            }

            foreach (var groupData in graphSnapshotData.groups)
            {
                var group = CreateGroup(groupData.position, groupData.key);

                foreach (var groupNodeKey in groupData.ownedNodesKeys)
                {
                    group.AddElement(actionNodes[groupNodeKey]);
                }
            }
        }

        public GraphSnapshotData SnapshotGraph()
        {
            var graphSnapshotData = new GraphSnapshotData();

            foreach (var element in graphElements)
            {
                if (element is ActionNode node)
                {
                    graphSnapshotData.nodes.Add(new NodeData()
                    {
                        key = node.Key,
                        metaData = node.MetaData,
                        transitions = node.Transitions.Clone(),
                        position = node.GetPosition().position
                    });
                }

                if (element is Group group)
                {
                    var groupData = new GroupData()
                    {
                        key = group.title,
                        position = group.GetPosition().position,
                        ownedNodesKeys = new List<string>()
                    };
                    
                    graphSnapshotData.groups.Add(groupData);

                    foreach (var groupElement in group.containedElements)
                    {
                        if (groupElement is ActionNode ownGroupNode)
                        {
                            groupData.ownedNodesKeys.Add(ownGroupNode.Key);
                        }
                    }
                }
            }

            return graphSnapshotData;
        }

        private Group CreateGroup(Vector2 position, string groupName = "DefaultGroupName")
        {
            var group = new Group()
            {
                title = groupName,
            };

            AddElement(group);
            
            group.SetPosition(new Rect(position, Vector2.one));

            foreach(var selected in selection)
            {
                if (selected is ActionNode graphNode)
                {
                    group.AddElement(graphNode);
                }
            }

            return group;
        }
        
        private ActionNode CreateNode(Vector2 position, string key = null, List<TransitionData> transitionsDates = null, string metaData = null)
        {
            var node = new ActionNode();
            key ??= _config.DefaultKeyNode;
            
            node.Initialize(key, transitionsDates, metaData);
            
            AddElement(node);
            
            node.SetPosition(new Rect(position, Vector2.one));

            RegisterNode(node);

            return node;
        }

        public void LookAt(string nodeKey)
        {
            foreach (var node in nodes)
            {
                if (node is ActionNode actionNode && actionNode.Key.Contains(nodeKey))
                {
                    UpdateViewTransform(contentViewContainer.ChangeCoordinatesTo(actionNode, viewport.contentRect.center - actionNode.contentRect.center), Vector3.one);
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            foreach (var port in ports)
            {
                if (startPort.node == port.node)
                {
                    continue;
                }
                
                if (startPort.direction == port.direction)
                {
                    continue;
                }
                
                compatiblePorts.Add(port);
            }
        
            return compatiblePorts;
        }

        private void AddStyles()
        {
            var graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("NovelGraph/NovelGraphViewStyles.uss");
            var nodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("NovelGraph/NovelNodeStyles.uss");
            
            styleSheets.Add(graphViewStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }

        private void AddGridBackground()
        {
            var gridBackground = new GridBackground();
            
            gridBackground.StretchToParentSize();
            
            Insert(0, gridBackground);
        }

        private void RegisterNode(ActionNode node)
        {
            node.ChangeKeyFunc += OnRevalidateKey;
            node.DeleteElementsRequestEvent += OnDeleteElements;
            
            AddKeyToMap(node);
        }

        private void OnDeleteElements(List<GraphElement> elements)
        {
            DeleteElements(elements);
        }

        private void AddKeyToMap(ActionNode node, string key = null)
        {
            key ??= node.Key;
            
            if (_keysMap.ContainsKey(key))
            {
                if (_keysMap[key].Count == 1)
                {
                    _keysMap[key][0].SetBackgroundColor(_config.ErrorColor);
                }
                
                _keysMap[key].Add(node);
                
                node.SetBackgroundColor(_config.ErrorColor);
            }
            else
            {
                _keysMap.Add(key, new List<ActionNode>() {node});
            }
        }

        private void RemoveKeyFromMap(ActionNode node, string key = null)
        {
            key ??= node.Key;
            
            _keysMap[key].Remove(node);
            
            node.SetBackgroundColor(_config.DefaultColor);

            switch (_keysMap[key].Count)
            {
                case 1:
                {
                    _keysMap[key][0].SetBackgroundColor(_config.DefaultColor);
                    break;
                }
                case 0:
                {
                    _keysMap.Remove(key);
                    break;
                }
            }
        }

        private string OnRevalidateKey(ActionNode node, string oldKey, string newKey)
        {
            if (_validator.TryValidate(newKey, out var result))
            {
                RemoveKeyFromMap(node, oldKey);
                AddKeyToMap(node, result);

                return result;
            }

            return _config.DefaultKeyNode;
        }
        
        // private void SetElementsDeletedCallback()
        // {
        //     deleteSelection = (_, _) =>
        //     {
        //         foreach (var selectable in selection)
        //         {
        //             if (selectable is not GraphNode graphNode)
        //             {
        //                 continue;
        //             }
        //             
        //             graphNode.ChangeKeyFunc -= OnRevalidateKey;
        //             graphNode.DeleteElementsRequestEvent -= OnDeleteElements;
        //             RemoveKeyFromMap(graphNode);
        //         }
        //
        //         DeleteSelection();
        //         // var connections = new HashSet<Edge>();
        //         //
        //         // foreach (var selectable in selection)
        //         // {
        //         //     if (selectable is not GraphNode graphNode)
        //         //     {
        //         //         continue;
        //         //     }
        //         //     
        //         //     graphNode.ChangeKeyFunc -= OnRevalidateKey;
        //         //     graphNode.DeleteElementsRequestEvent -= OnDeleteElements;
        //         //     RemoveKeyFromMap(graphNode);
        //         //     
        //         //     graphNode.Query<Port>().ForEach(port =>
        //         //     {
        //         //         if (!port.connected)
        //         //         {
        //         //             return;
        //         //         }
        //         //
        //         //         connections.UnionWith(port.connections);
        //         //     });
        //         // }
        //         //
        //         // connections.Remove(null);
        //         //
        //         // DeleteElements(connections);
        //         // DeleteElements(selection.OfType<GraphElement>());
        //     };
        // }

        private void SetGraphViewChangedCallback()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (var edge in changes.edgesToCreate)
                    {
                        var node = (ActionNode)edge.input.node;

                        var transitionData = (TransitionData)edge.output.userData;

                        transitionData.nodeKey = node.Key;
                    }
                }
                
                if (changes.elementsToRemove != null)
                {
                    foreach (var element in changes.elementsToRemove)
                    {
                        switch (element)
                        {
                            case Edge edge:
                            {
                                var transitionData = (TransitionData)edge.output.userData;
                                transitionData.nodeKey = string.Empty;
                                break;
                            }
                            case ActionNode actionNode:
                            {
                                actionNode.ChangeKeyFunc -= OnRevalidateKey;
                                actionNode.DeleteElementsRequestEvent -= OnDeleteElements;
                                RemoveKeyFromMap(actionNode);
                                break;
                            }
                        }
                    }
                }

                return changes;
            };
        }
        
        public void SwitchActiveMiniMap()
        {
            _miniMap.visible = !_miniMap.visible;
        }
        
        private Vector2 GetGraphMousePosition(Vector2 mousePosition) => contentViewContainer.WorldToLocal(mousePosition);

        public void DeleteAll()
        {
            DeleteElements(graphElements);
        }
    }
}