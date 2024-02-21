using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class HubProcessor : IActionProcessor
    {
        [SerializeReference] private IProcessRunner _runner;
        [SerializeReference] private List<IActionProcessor> _processors = new();

        public HubProcessor()
        {
        }
        
        protected HubProcessor(HubProcessor data)
        {
            _runner = data._runner.Clone();
            _processors = data._processors.Select(processor => processor.Clone()).ToList();
        }

        [Inject]
        public void Construct(DiContainer container)
        {
            foreach (var processor in _processors)
            {
                container.Inject(processor);
            }
        }

        public IActionProcessor Clone()
        {
            return new HubProcessor(this);
        }
        
        public UniTask RunProcess(CancellationToken token = default)
        {
            return _runner.RunProcess(_processors.Select(processor => processor.RunProcess(token)));
        }
    }
}