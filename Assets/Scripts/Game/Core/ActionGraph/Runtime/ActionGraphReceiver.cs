using System;
using System.Collections.Generic;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionGraphReceiver
    {
        private ActionGraphData _graphData;
        private Dictionary<string, NodeData> _graph;
        
        public event Action<NodeData> SwitchNodeEvent;
        
        public NodeData CurrentNode { get; private set; }

        public ActionGraphReceiver(ActionGraphData data)
        {
            _graphData = data;
            
            _graph = new Dictionary<string, NodeData>();

            foreach (var nodeData in _graphData.GetSnapshotData().nodes)
            {
                _graph.Add(nodeData.key, nodeData);
            }
        }

        public void SwitchToNode(string nodeId)
        {
            CurrentNode = _graph[nodeId];
            
            SwitchNodeEvent?.Invoke(CurrentNode);
        }
        
        public void SwitchToNextNode(string transitionId)
        {
            SwitchToNode(CurrentNode.transitions.Find(data => data.value == transitionId).nodeKey);
        }
        
        public void SwitchToNextNode(int transitionIndex)
        {
            SwitchToNode(CurrentNode.transitions[transitionIndex].nodeKey);
        }
    }
}