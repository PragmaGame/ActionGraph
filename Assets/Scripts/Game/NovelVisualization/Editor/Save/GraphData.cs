using System.Collections.Generic;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    public class GraphData : ScriptableObject
    {
        public List<GroupData> groups;
        public List<NodeData> nodes;
    }
}