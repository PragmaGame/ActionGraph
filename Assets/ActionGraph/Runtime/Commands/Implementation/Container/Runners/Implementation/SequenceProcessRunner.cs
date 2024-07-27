using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Pragma.ActionGraph.Runtime.Commands
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