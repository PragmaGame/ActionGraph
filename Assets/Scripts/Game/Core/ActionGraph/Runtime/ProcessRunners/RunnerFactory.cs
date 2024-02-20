using System;

namespace Game.Core.ActionGraph.Runtime
{
    public class RunnerFactory
    {
        public IProcessRunner Create(RunnerType runnerType)
        {
            return runnerType switch
            {
                RunnerType.Parallel => new ParallelProcessRunner(),
                RunnerType.Sequence => new SequenceProcessRunner(),
                _ => throw new ArgumentOutOfRangeException(nameof(runnerType), runnerType, null)
            };
        }
    }
}