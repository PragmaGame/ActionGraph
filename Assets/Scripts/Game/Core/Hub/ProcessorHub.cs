using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.ActionGraph.Runtime;
using Game.Core.Hub.Processors;
using Game.Core.Hub.ProcessRunners;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub
{
    [Serializable]
    public class ProcessorHub
    {
        [SerializeReference] private IProcessRunner _processRunner = new ParallelProcessRunner();
        [SerializeReference] private List<IActionProcessor> _processors = new();

        [Inject]
        private void Construct(DiContainer container)
        {
            foreach (var processor in _processors)
            {
                container.Inject(processor);
            }
        }

        public UniTask RunProcess(CancellationToken token)
        {
            return _processRunner.RunProcess(_processors.Select(x => x.RunProcess(token)));
        }
    }
}