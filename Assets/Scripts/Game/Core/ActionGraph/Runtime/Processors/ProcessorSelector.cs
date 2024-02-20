using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.ActionGraph.Runtime
{
    public class ProcessorSelector
    {
        private readonly Dictionary<Type, IActionProcessor> _map;

        public ProcessorSelector(IEnumerable<IActionProcessor> processors)
        {
            _map = new Dictionary<Type, IActionProcessor>();

            foreach (var processor in processors)
            {
                _map.Add(processor.DataType, processor);
            }
        }

        public UniTask Select(IActionProcessorData data, CancellationToken token = default)
        {
            var type = data.GetType();
            return _map[type].RunProcess(data, token);
        }
        
        public IEnumerable<UniTask> Select(List<IActionProcessorData> dates, CancellationToken token = default)
        {
            return dates.Select(data =>
            {
                var type = data.GetType();
                return _map[type].RunProcess(data, token);
            });
        }
    }
}