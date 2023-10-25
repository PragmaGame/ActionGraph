using System;
using UnityEngine;

namespace Game.NovelVisualization.Runtime
{
    [Serializable]
    public class DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DSDialogueSO NextDialogue { get; set; }
    }
}