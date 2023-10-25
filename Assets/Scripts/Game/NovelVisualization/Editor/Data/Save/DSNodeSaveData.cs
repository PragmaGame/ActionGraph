using System;
using System.Collections.Generic;
using Game.NovelVisualization.Runtime;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    [Serializable]
    public class DSNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public List<TransitionSaveData> Choices { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public TransitionType DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}