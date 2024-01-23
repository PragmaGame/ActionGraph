using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class GroupData
    {
        public string key;
        public List<string> ownedNodesKeys;
        
        public Vector2 position;
    }
}