using System;
using System.Collections.Generic;
using Pragma.ActionGraph.Runtime.Common;
using UnityEngine;

namespace Pragma.ActionGraph.Editor.Validators
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