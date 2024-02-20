using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.ActionGraph.Runtime
{
    public class ProcessorContainer : ActionProcessor<ProcessorContainerData>
    {
        private RunnerFactory _runnerFactory;
        private ProcessorSelector _selector;
        
        public ProcessorContainer(ProcessorSelector selector, RunnerFactory runnerFactory)
        {
            _runnerFactory = runnerFactory;
            _selector = selector;
        }

        public override UniTask RunProcess(ProcessorContainerData data, CancellationToken token = default)
        {
            var runner = _runnerFactory.Create(data.runnerType);
            
            return runner.RunProcess(_selector.Select(data.dates, token));
        }
    }
}