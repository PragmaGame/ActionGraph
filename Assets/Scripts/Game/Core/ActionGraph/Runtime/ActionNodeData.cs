using System.Collections.Generic;
using Game.Core.Hub;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionNodeData : ScriptableObject
    {
        [field: SerializeField,ReadOnly] public string Key { get; set; }
        [field: SerializeField,ReadOnly] public Vector2 Position {get; set; }

        [field: SerializeField, ListDrawerSettings(IsReadOnly = true)]
        public List<TransitionData> Transitions { get; set; } = new();

        [Space]
        [Header("Processors")]
        [SerializeField] private List<ProcessorHub> _processorHubs;
        [SerializeField] private TransitionProcessor _transitionProcessor;

        public IReadOnlyList<ProcessorHub> ProcessorHubs => _processorHubs;
    }
}