using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [CreateAssetMenu(fileName = nameof(ActionGraphData), menuName = "ActionGraph/" + nameof(ActionGraphData))]
    public class ActionGraphData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        
        [ReadOnly, SerializeField] private List<GroupData> _groups;
        [ReadOnly, SerializeField] private List<ActionNodeData> _nodes;

        [field: SerializeField, ReadOnly] public Rect LastPosition { get; set; }
        
        public List<ActionNodeData> Nodes => _nodes;
        public List<GroupData> Groups => _groups;
    }
}