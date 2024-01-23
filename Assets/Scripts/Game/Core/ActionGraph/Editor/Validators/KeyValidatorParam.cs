using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Core.ActionGraph.Editor
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