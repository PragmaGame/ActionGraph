using System;
using UnityEngine;

namespace Game.Graph
{
    public class GameGraph : MonoBehaviour
    {
        [SerializeField] private NodeMetaDataParser _parser;
        
        public event Action<string> ChangeNodeGraphEvent;

        public void SwitchToNode(string nodeId)
        {
            
        }
        
        public void SwitchToNextNode(string nodeId)
        {
            
        }
    }
}