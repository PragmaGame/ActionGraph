using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionGraphData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set;}
        
        [SerializeField] private GraphSnapshotData _graphSnapshotData;

        public void Reset()
        {
            Id = name;
        }
        
        public GraphSnapshotData GetSnapshotData()
        {
            return _graphSnapshotData.Clone();
        }

#if UNITY_EDITOR
        public void SetSnapshotData(GraphSnapshotData graphSnapshotData)
        {
            _graphSnapshotData = graphSnapshotData;
        }

        public GraphSnapshotData GetOriginalData()
        {
            return _graphSnapshotData;
        }
#endif
    }
}