using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Hub.Processors;
using Game.Core.Hub.ProcessRunners;
using UnityEngine;

namespace Game.Core.Hub
{
    [Serializable]
    public class ProcessorHub
    {
        [SerializeReference] private IProcessRunner _processRunner;
        [SerializeReference] private List<IActionProcessor> _processors;

        public void Construct()
        {
            foreach (var processor in _processors)
            {
                processor.Construct();
            }
        }

        public UniTask RunProcess(CancellationToken token)
        {
            return _processRunner.RunProcess(_processors.Select(x => x.RunProcess(token)));
        }
    }
}