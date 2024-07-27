using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Pragma.ActionGraph.Runtime.Commands
{
    public interface IProcessRunner
    {
        public UniTask RunProcess(IEnumerable<UniTask> processors);
        public IProcessRunner Clone();
    }
}