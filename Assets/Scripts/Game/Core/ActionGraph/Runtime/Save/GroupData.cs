using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class GroupData
    {
        public string Key { get; set; }
        public List<string> OwnedNodesKeys { get; set; }
        public Vector2 Position { get; set; }
    }
}