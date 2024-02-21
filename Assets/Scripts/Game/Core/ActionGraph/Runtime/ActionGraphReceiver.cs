using System;
using System.Collections.Generic;
using System.Threading;
using Zenject;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionGraphReceiver
    {
        private ActionGraphData _graphData;
        private DiContainer _container;
        private ActionNodeSelector _actionNodeSelector;
        
        private Dictionary<string, ActionNodeData> _graph;
        private CancellationTokenSource _runToken;

        public event Action<ActionNodeData> SwitchNodeEvent;
        
        public ActionNodeData CurrentActionNodeData { get; private set; }

        public ActionGraphReceiver(ActionGraphData data, DiContainer container, ActionNodeSelector actionNodeSelector)
        {
            _graphData = data;
            _container = container;
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

            var processor = CurrentActionNodeData.Processor.Clone();
            
            _container.Inject(processor);
            
            await processor.RunProcess(_runToken.Token);

            var transitionData = await _actionNodeSelector.SelectNode(CurrentActionNodeData.Transitions);
            
            SwitchToNode(transitionData.nodeKey);
        }
    }
}