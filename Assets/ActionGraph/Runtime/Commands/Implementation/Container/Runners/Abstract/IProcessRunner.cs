using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace ActionGraph.Runtime.Commands.Implementation.Container.Runners.Abstract
{
    public interface IProcessRunner
    {
        public UniTask RunProcess(IEnumerable<UniTask> processors);
        public IProcessRunner Clone();
    }
}