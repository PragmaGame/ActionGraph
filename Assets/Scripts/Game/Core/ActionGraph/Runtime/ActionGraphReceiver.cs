using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionGraphReceiver : MonoBehaviour
    {
        [SerializeField] private ActionGraphData _actionGraphData;

        private Dictionary<string, NodeData> _graph;
        
        public event Action<NodeData> SwitchNodeEvent;
        
        public NodeData CurrentNode { get; private set; }
        
        private void Awake()
        {
            _graph = new Dictionary<string, NodeData>();

            foreach (var data in _actionGraphData.GetSnapshotData().nodes)
            {
                _graph.Add(data.key, data);
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