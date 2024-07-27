using System;
using System.Collections.Generic;
using System.Threading;
using Pragma.ActionGraph.Runtime.Save;
using Zenject;

namespace Pragma.ActionGraph.Runtime
{
    public class ActionGraphInvoker
    {
        private ActionGraphData _graphData;
        private DiContainer _container;

        private Dictionary<string, ActionNodeData> _graph;
        private CancellationTokenSource _invokeToken;

        public event Action<ActionNodeData> SwitchNodeEvent;
        
        public ActionNodeData CurrentActionNodeData { get; private set; }

        public ActionGraphInvoker(ActionGraphData data, DiContainer container)
        {
            _graphData = data;
            _container = container;

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
            
            InvokeAction(CurrentActionNodeData);
        }

        public async void InvokeAction(ActionNodeData data)
        {
            _invokeToken = new CancellationTokenSource();

            var currentCommand = data.Command.Clone();
            
            _container.Inject(currentCommand);
            
            await currentCommand.Execute(_invokeToken.Token);

            // TODO NextNodeSelector
            var transitionData = CurrentActionNodeData.Transitions[0];
            
            SwitchToNode(transitionData.nodeKey);
        }

        public void CancelInvoke()
        {
            _invokeToken?.Cancel();
        }
    }
}