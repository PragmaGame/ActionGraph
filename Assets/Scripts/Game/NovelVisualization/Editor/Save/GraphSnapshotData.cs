using System;
using System.Collections.Generic;

namespace Game.NovelVisualization.Editor
{
    [Serializable]
    public class GraphSnapshotData
    {
        public List<GroupData> groups;
        public List<NodeData> nodes;

        public GraphSnapshotData()
        {
            groups = new List<GroupData>();
            nodes = new List<NodeData>();
        }
    }
}