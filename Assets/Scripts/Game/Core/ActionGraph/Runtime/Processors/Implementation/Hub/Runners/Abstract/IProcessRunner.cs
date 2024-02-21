using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Core.ActionGraph.Runtime
{
    public interface IProcessRunner
    {
        public UniTask RunProcess(IEnumerable<UniTask> processors);
        public IProcessRunner Clone();
    }
}