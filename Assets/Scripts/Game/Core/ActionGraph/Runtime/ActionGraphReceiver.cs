using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionGraphReceiver
    {
        private ActionGraphData _graphData;
        private ProcessorSelector _selector;
        private ActionNodeSelector _actionNodeSelector;
        
        private Dictionary<string, ActionNodeData> _graph;
        private CancellationTokenSource _runToken;

        public event Action<ActionNodeData> SwitchNodeEvent;
        
        public ActionNodeData CurrentActionNodeData { get; private set; }

        public ActionGraphReceiver(ActionGraphData data, ProcessorSelector selector, ActionNodeSelector actionNodeSelector)
        {
            _graphData = data;
            _selector = selector;
            _actionNodeSelector = actionNodeSelector;
            
            _graph = new Dictionary<string, ActionNodeData>();

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
            
            InvokeAction();
        }

        public void SwitchToNextNode(int transitionIndex = 0)
        {
            SwitchToNode(CurrentActionNodeData.Transitions[transitionIndex].nodeKey);
        }

        private async void InvokeAction()
        {
            _runToken = new CancellationTokenSource();
            
            await _selector.Select(CurrentActionNodeData.Data);

            var transitionData = await _actionNodeSelector.SelectNode(CurrentActionNodeData.Transitions);
            
            SwitchToNode(transitionData.nodeKey);
        }
    }
}