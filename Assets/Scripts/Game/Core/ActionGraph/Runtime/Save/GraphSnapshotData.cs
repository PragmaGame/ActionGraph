using System;
using System.Collections.Generic;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class GraphSnapshotData
    {
        public List<GroupData> groups;
        public List<NodeData> nodes;

        public Dictionary<string, NodeData> CacheNodes;

        public GraphSnapshotData()
        {
            groups = new List<GroupData>();
            nodes = new List<NodeData>();
        }

        public void CreateCacheNodes()
        {
            CacheNodes = new Dictionary<string, NodeData>();

            foreach (var node in nodes)
            {
                CacheNodes.Add(node.key, node);
            }
        }
    }
}