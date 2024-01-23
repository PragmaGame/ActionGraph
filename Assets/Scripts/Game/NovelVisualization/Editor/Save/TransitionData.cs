using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    [Serializable]
    public class TransitionData
    {
        public string value;
        public string nodeKey;
    }

    public static class TransitionDataExtensions
    {
        public static List<TransitionData> Clone(this List<TransitionData> data) => data
            .Select(data => new TransitionData { value = data.value, nodeKey = data.nodeKey }).ToList();
    }
} 