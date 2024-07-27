using System;
using System.Collections.Generic;
using ActionGraph.Runtime.Commands.Implementation.Container.Runners.Abstract;
using Cysharp.Threading.Tasks;

namespace ActionGraph.Runtime.Commands.Implementation.Container.Runners.Implementation
{
    [Serializable]
    public class SequenceProcessRunner : IProcessRunner
    {
        public async UniTask RunProcess(IEnumerable<UniTask> processors)
        {
            foreach (var processor in processors)
            {
                await processor;
            }
        }

        public IProcessRunner Clone() => new SequenceProcessRunner();
    }
}