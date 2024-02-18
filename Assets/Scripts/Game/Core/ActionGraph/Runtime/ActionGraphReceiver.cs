using System;
using System.Collections.Generic;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionGraphReceiver
    {
        private ActionGraphData _graphData;
        private Dictionary<string, ActionNodeData> _graph;

        private Dictionary<string, Action> _actionMap;

        public event Action<ActionNodeData> SwitchNodeEvent;
        
        public ActionNodeData CurrentActionNodeData { get; private set; }

        public ActionGraphReceiver(ActionGraphData data)
        {
            _graphData = data;
            
            _graph = new Dictionary<string, ActionNodeData>();
            _actionMap = new Dictionary<string, Action>();

            foreach (var nodeData in _graphData.Nodes)
            {
                _graph.Add(nodeData.Key, nodeData);
            }
        }

        public void SwitchToNode(string nodeId)
        {
            if (!_graph.ContainsKey(nodeId))
            {
                return;
            }
            
            CurrentActionNodeData = _graph[nodeId];
            
            SwitchNodeEvent?.Invoke(CurrentActionNodeData);
            
            InvokeSwitchConcreteNode(nodeId);
        }

        public void SwitchToNextNode(int transitionIndex = 0)
        {
            SwitchToNode(CurrentActionNodeData.Transitions[transitionIndex].nodeKey);
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