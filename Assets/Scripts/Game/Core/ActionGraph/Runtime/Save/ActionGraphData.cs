using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [CreateAssetMenu(fileName = nameof(ActionGraphData), menuName = "ActionGraph/" + nameof(ActionGraphData))]
    public class ActionGraphData : ScriptableObject
    {
        [ReadOnly, SerializeField] private List<GroupData> _groups;
        [ReadOnly, SerializeField] private List<ActionNodeData> _nodes;

        public List<ActionNodeData> Nodes => _nodes;
        public List<GroupData> Groups => _groups;
    }
}