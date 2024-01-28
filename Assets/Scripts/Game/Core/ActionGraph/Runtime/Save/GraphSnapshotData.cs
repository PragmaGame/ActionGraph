using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.ActionGraph.Runtime
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
        
        protected GraphSnapshotData(GraphSnapshotData data)
        {
            groups = data.groups.Select(group => group.Clone()).ToList();
            nodes = data.nodes.Select(node => node.Clone()).ToList();
        }

        public GraphSnapshotData Clone() => new(this);
    }
}