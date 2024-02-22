using System.Collections.Generic;
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

        [field: Space]
        [SerializeReference] private IActionCommand _command = new ContainerCommand();

        public IActionCommand Command => _command;
    }
}