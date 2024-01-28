using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class TransitionData
    {
        public string value;
        public string nodeKey;
    }

    public static class TransitionDataExtensions
    {
        public static List<TransitionData> Clone(this List<TransitionData> transitionData) => transitionData
            .Select(data => new TransitionData { value = data.value, nodeKey = data.nodeKey }).ToList();
    }
} 