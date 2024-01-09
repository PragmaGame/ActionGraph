using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.NovelVisualization.Editor
{
    public class NovelGraphView : GraphView
    {
        private NovelGraphViewConfig _config;

        private Dictionary<string, List<GraphNode>> _uniqueNodesKeys;

        public NovelGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();

            SetElementsDeletedCallback();
            SetGraphViewChangedCallback();
            
            _config = (NovelGraphViewConfig)EditorGUIUtility.Load("NovelGraph/NovelGraphViewConfig.asset");

            _uniqueNodesKeys = new Dictionary<string, List<GraphNode>>();
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
        
        private IManipulator CreateNodeContextMenu()
        {
            var contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.InsertAction(0,"Create Node", 
                    action => AddElement(CreateNode(GetGraphMousePosition(action.eventInfo.localMousePosition))))
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

        private Group CreateGroup(Vector2 position)
        {
            var group = new Group()
            {
                title = "DefaultGroupName"
            };

            AddElement(group);
            
            group.SetPosition(new Rect(position, Vector2.one));

            foreach(var selected in selection)
            {
                if (selected is GraphNode graphNode)
                {
                    group.AddElement(graphNode);
                }
            }

            return group;
        }
        
        private GraphNode CreateNode(Vector2 position)
        {
            var node = new GraphNode();
            
            node.Initialize(position);
            node.Draw();

            RegisterNode(node);

            return node;
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

        private void RegisterNode(GraphNode node)
        {
            node.ChangeKeyEvent += OnRevalidateKey;
            node.DeleteElementsRequestEvent += OnDeleteElements;
            
            ValidateKey(node);
        }

        private void OnDeleteElements(List<GraphElement> elements)
        {
            DeleteElements(elements);
        }

        private void ValidateKey(GraphNode node)
        {
            var key = node.Key;
            
            if (_uniqueNodesKeys.ContainsKey(key))
            {
                if (_uniqueNodesKeys[key].Count == 1)
                {
                    _uniqueNodesKeys[key][0].SetBackgroundColor(_config.ErrorColor);
                }
                
                _uniqueNodesKeys[key].Add(node);
                
                node.SetBackgroundColor(_config.ErrorColor);
            }
            else
            {
                _uniqueNodesKeys.Add(node.Key, new List<GraphNode>() {node});
            }
        }

        private void RemoveKey(GraphNode node, string key = null)
        {
            key ??= node.Key;
            
            _uniqueNodesKeys[key].Remove(node);
            
            node.SetBackgroundColor(_config.DefaultColor);

            switch (_uniqueNodesKeys[key].Count)
            {
                case 1:
                {
                    _uniqueNodesKeys[key][0].SetBackgroundColor(_config.DefaultColor);
                    break;
                }
                case 0:
                {
                    _uniqueNodesKeys.Remove(key);
                    break;
                }
            }
        }

        private void OnRevalidateKey(GraphNode node, string oldKey)
        {
            RemoveKey(node, oldKey);
            
            ValidateKey(node);
        }
        
        private void SetElementsDeletedCallback()
        {
            deleteSelection = (_, _) =>
            {
                var connections = new HashSet<Edge>();
                
                foreach (var selectable in selection)
                {
                    if (selectable is not GraphNode graphNode)
                    {
                        continue;
                    }
                    
                    graphNode.ChangeKeyEvent -= OnRevalidateKey;
                    graphNode.DeleteElementsRequestEvent -= OnDeleteElements;
                    RemoveKey(graphNode);
                    
                    graphNode.Query<Port>().ForEach(port =>
                    {
                        if (!port.connected)
                        {
                            return;
                        }

                        connections.UnionWith(port.connections);
                    });
                }

                connections.Remove(null);
                
                DeleteElements(connections);
                DeleteElements(selection.OfType<GraphElement>());
            };
        }

        private void SetGraphViewChangedCallback()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (var edge in changes.edgesToCreate)
                    {
                        var node = (GraphNode)edge.input.node;

                        var transitionData = (TransitionData)edge.output.userData;

                        transitionData.nodeKey = node.Key;
                    }
                }
                
                if (changes.elementsToRemove != null)
                {
                    foreach (var element in changes.elementsToRemove)
                    {
                        if (element is Edge edge)
                        {
                            var transitionData = (TransitionData)edge.output.userData;

                            transitionData.nodeKey = string.Empty;
                        }
                    }
                }

                return changes;
            };
        }
        
        private Vector2 GetGraphMousePosition(Vector2 mousePosition) => contentViewContainer.WorldToLocal(mousePosition);
    }
}