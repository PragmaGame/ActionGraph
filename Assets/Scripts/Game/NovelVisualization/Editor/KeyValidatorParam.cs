using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    [Serializable]
    public class KeyValidatorParam
    {
        [SerializeField] private SerializedTuple<string, string>[] _patterns = new []
        {
            new SerializedTuple<string, string>(@"\s+", "-"),
            new SerializedTuple<string, string>(@"[^a-z0-9\-]", "")
        };

        public IReadOnlyList<SerializedTuple<string, string>> Patterns => _patterns;
    }
}