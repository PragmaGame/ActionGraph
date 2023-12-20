using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Hub.ProcessRunners
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
    }
}