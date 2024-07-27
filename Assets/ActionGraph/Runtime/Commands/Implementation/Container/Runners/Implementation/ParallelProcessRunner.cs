using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Pragma.ActionGraph.Runtime.Commands
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