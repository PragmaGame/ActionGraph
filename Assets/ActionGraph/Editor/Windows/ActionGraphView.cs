using System;
using System.Collections.Generic;
using System.Linq;
using ActionGraph.Editor.Elements;
using ActionGraph.Runtime.Save;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace ActionGraph.Editor.Windows
{
    public class ActionGraphView : GraphView
    {
        private ActionGraphViewConfig _config;
        private ActionGraphData _rootData;
        
        private MiniMap _miniMap;

        private List<ActionNodeData> _nodesToRemove;
        private List<ActionNodeData> _nodesToAdded;

        public ActionGraphView(ActionGraphViewConfig config)
        {
            _config = config;
            
            AddManipulators();
            AddGridBackground();
            AddMiniMap();
            AddStyles();
            
            SetGraphViewChangedCallback();

            _nodesToAdded = new List<ActionNodeData>();
            _nodesToRemove = new List<ActionNodeData>();
        }

        public void Save()
        {
            // Remove nodes
            foreach (var data in _nodesToRemove)
            {
                _rootData.Nodes.Remove(data);
                Object.DestroyImmediate(data, true);
            }
            
            _nodesToRemove.Clear();

            // foreach (var actionNode in _rootData.Nodes)
            // {
            //     EditorUtility.SetDirty(actionNode);
            // }
            
            // Added nodes
            foreach (var data in _nodesToAdded)
            {
                AssetDatabase.AddObjectToAsset(data, _rootData);
                _rootData.Nodes.Add(data);
            }
            
            _nodesToAdded.Clear();

            //Save groups
            _rootData.Groups.Clear();
            
            foreach (var graphElement in graphElements)
            {
                if (graphElement is Group group)
                {
                    var groupData = new GroupData()
                    {
                        Key = group.title,
                        Position = group.GetPosition().position,
                        OwnedNodesKeys = new List<string>()
                    };
    
                    _rootData.Groups.Add(groupData);
                    
                    foreach (var groupElement in group.containedElements)
                    {
                        if (groupElement is ActionNode ownGroupNode)
                        {
                            groupData.OwnedNodesKeys.Add(ownGroupNode.Key);
                        }
                    }
                }
            }

            _rootData.LastPosition = new Rect(contentViewContainer.transform.position, contentViewContainer.transform.scale);
            
            EditorUtility.SetDirty(_rootData);
            AssetDatabase.SaveAssetIfDirty(_rootData);
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
        
        public void LoadData(ActionGraphData graphData)
        {
            _rootData = graphData;
            
            var actionNodes = new Dictionary<string, ActionNode>();

            foreach (var nodeData in graphData.Nodes)
            {
                var node = CreateNode(data: nodeData);
                
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

            foreach (var groupData in graphData.Groups)
            {
                var group = CreateGroup(groupData.Position, groupData.Key);

                foreach (var groupNodeKey in groupData.OwnedNodesKeys)
                {
                    group.AddElement(actionNodes[groupNodeKey]);
                }
            }
            
            UpdateViewTransform(_rootData.LastPosition.position, _rootData.LastPosition.size);
        }

        // public GraphSnapshotData SnapshotGraph()
        // {
        //     var graphSnapshotData = new GraphSnapshotData();
        //
        //     foreach (var element in graphElements)
        //     {
        //         if (element is ActionNode node)
        //         {
        //             graphSnapshotData.nodes.Add(node.Data);
        //         }
        //
        //         if (element is Group group)
        //         {
        //             var groupData = new GroupData()
        //             {
        //                 key = group.title,
        //                 position = group.GetPosition().position,
        //                 ownedNodesKeys = new List<string>()
        //             };
        //             
        //             graphSnapshotData.groups.Add(groupData);
        //
        //             foreach (var groupElement in group.containedElements)
        //             {
        //                 if (groupElement is ActionNode ownGroupNode)
        //                 {
        //                     groupData.ownedNodesKeys.Add(ownGroupNode.Key);
        //                 }
        //             }
        //         }
        //     }
        //
        //     return graphSnapshotData;
        // }

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
        
        private ActionNode CreateNode(Vector2 position = default, ActionNodeData data = null)
        {
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<ActionNodeData>();
                data.name = _config.DefaultTagNode;
                data.Tag = _config.DefaultTagNode;
                data.Key = Guid.NewGuid().ToString();
                data.Position = position;
                
                _nodesToAdded.Add(data);
            }
            else
            {
                position = data.Position;
            }

            var node = new ActionNode(data);

            AddElement(node);
            
            node.SetPosition(new Rect(position, Vector2.one));

            node.DeleteElementsRequestEvent += OnDeleteElements;

            return node;
        }

        public void LookAt(string value)
        {
            foreach (var element in graphElements)
            {
                switch (element)
                {
                    case Group group when group.title.Contains(value):
                    {
                        LookAt(group);
                        return;
                    }
                    case ActionNode node when node.Tag.Contains(value):
                    {
                        LookAt(node);
                        return;
                    }
                }
            }
        }

        public void LookAt(VisualElement element)
        {
            UpdateViewTransform(contentViewContainer
                .ChangeCoordinatesTo(element, viewport.contentRect.center - element.contentRect.center), Vector3.one);
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
            foreach (var styleSheet in _config.StyleSheet)
            {
                styleSheets.Add(styleSheet);
            }
        }

        private void AddGridBackground()
        {
            var gridBackground = new GridBackground();
            
            gridBackground.StretchToParentSize();
            
            Insert(0, gridBackground);
        }
        
        private void OnDeleteElements(List<GraphElement> elements)
        {
            DeleteElements(elements);
        }

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
                                actionNode.DeleteElementsRequestEvent -= OnDeleteElements;
                                _nodesToRemove.Add(actionNode.Data);
                                _nodesToAdded.Remove(actionNode.Data);
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