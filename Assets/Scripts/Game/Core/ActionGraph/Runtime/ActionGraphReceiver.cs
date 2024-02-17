using System;
using System.Collections.Generic;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionGraphReceiver
    {
        private ActionGraphData _graphData;
        private Dictionary<string, NodeData> _graph;

        private Dictionary<string, Action> _actionMap;

        public event Action<NodeData> SwitchNodeEvent;
        
        public NodeData CurrentNode { get; private set; }

        public ActionGraphReceiver(ActionGraphData data)
        {
            _graphData = data;
            
            _graph = new Dictionary<string, NodeData>();
            _actionMap = new Dictionary<string, Action>();

            foreach (var nodeData in _graphData.GetSnapshotData().nodes)
            {
                _graph.Add(nodeData.key, nodeData);
            }
        }

        public void SwitchToNode(string nodeId)
        {
            if (!_graph.ContainsKey(nodeId))
            {
                return;
            }
            
            CurrentNode = _graph[nodeId];
            
            SwitchNodeEvent?.Invoke(CurrentNode);
            
            InvokeSwitchConcreteNode(nodeId);
        }

        public void SwitchToNextNode(string transitionId)
        {
            SwitchToNode(CurrentNode.transitions.Find(data => data.value == transitionId).nodeKey);
        }
        
        public void SwitchToNextNode(int transitionIndex = 0)
        {
            SwitchToNode(CurrentNode.transitions[transitionIndex].nodeKey);
        }

        public void SubscribeToSwitchConcreteNode(string nodeKey, Action action)
        {
            if (_actionMap.ContainsKey(nodeKey))
            {
                _actionMap[nodeKey] += action;
            }
            else
            {
                _actionMap.Add(nodeKey, action);
            }
        }
        
        public void UnsubscribeToSwitchConcreteNode(string nodeKey, Action action)
        {
            if (_actionMap.ContainsKey(nodeKey))
            {
                _actionMap[nodeKey] -= action;
            }
        }

        private void InvokeSwitchConcreteNode(string nodeKey)
        {
            if (_actionMap.ContainsKey(nodeKey))
            {
                _actionMap[nodeKey]?.Invoke();
            }
        }
    }
}