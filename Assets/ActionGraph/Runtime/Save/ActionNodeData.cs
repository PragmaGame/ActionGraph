using System.Collections.Generic;
using Pragma.ActionGraph.Runtime.Commands;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pragma.ActionGraph.Runtime.Save
{
    public class ActionNodeData : ScriptableObject
    {
        [field: SerializeField,ReadOnly] public string Key { get; set; }
        [field: SerializeField,ReadOnly] public Vector2 Position {get; set; }

        [field: SerializeField] public string Tag { get; set; }
        
        [field: SerializeField, ListDrawerSettings(IsReadOnly = true)]
        public List<TransitionData> Transitions { get; set; } = new();

        [field: Space]
        [SerializeReference] private IActionCommand _command = new ContainerCommand();

        public IActionCommand Command => _command;
    }
}