﻿using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    [CreateAssetMenu(fileName = nameof(ActionGraphEditorConfig), menuName = "ActionGraph/" + nameof(ActionGraphEditorConfig))]
    public class ActionGraphEditorConfig : ScriptableObject
    {
        [field: SerializeField] public StyleSheet Variables { get; private set; }
        
        [field: SerializeField] public ActionGraphViewConfig ActionGraphViewConfig { get; private set; }
        [field: SerializeField] public ActionGraphToolBarViewConfig ActionGraphToolBarViewConfig { get; private set; }
    }
}