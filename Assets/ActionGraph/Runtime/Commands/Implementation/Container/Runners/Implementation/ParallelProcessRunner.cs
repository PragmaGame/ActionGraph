using System;
using System.Collections.Generic;
using ActionGraph.Runtime.Commands.Implementation.Container.Runners.Abstract;
using Cysharp.Threading.Tasks;

namespace ActionGraph.Runtime.Commands.Implementation.Container.Runners.Implementation
{
    [Serializable]
    public class ParallelProcessRunner : IProcessRunner
    {
        public async UniTask RunProcess(IEnumerable<UniTask> processors)
        {
            await UniTask.WhenAll(processors);
        }
        
        public IProcessRunner Clone() => new ParallelProcessRunner();
    }
}