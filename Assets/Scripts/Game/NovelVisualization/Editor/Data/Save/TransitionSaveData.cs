using System;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    [Serializable]
    public class TransitionSaveData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
    }
}