using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActionGraph.Runtime.Save
{
    [Serializable]
    public class GroupData
    {
        public string Key { get; set; }
        public List<string> OwnedNodesKeys { get; set; }
        public Vector2 Position { get; set; }
    }
}