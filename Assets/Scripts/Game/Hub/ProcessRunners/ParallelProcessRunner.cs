using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Hub.ProcessRunners
{
    [Serializable]
    public class ParallelProcessRunner : IProcessRunner
    {
        public async UniTask RunProcess(IEnumerable<UniTask> processors)
        {
            await UniTask.WhenAll(processors);
        }
    }
}