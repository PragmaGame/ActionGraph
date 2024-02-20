using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class ProcessorContainerData : IActionProcessorData
    {
        public RunnerType runnerType = RunnerType.Sequence;
        [SerializeReference] public List<IActionProcessorData> dates = new();
    }
}