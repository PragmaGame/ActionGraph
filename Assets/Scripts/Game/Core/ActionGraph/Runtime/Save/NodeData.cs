using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class NodeData
    {
        public string key;
        public string metaData;
        public List<TransitionData> transitions;
        
        public Vector2 position;
    }
}