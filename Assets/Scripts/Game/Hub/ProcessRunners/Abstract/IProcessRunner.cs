using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Hub.ProcessRunners
{
    public interface IProcessRunner
    {
        public UniTask RunProcess(IEnumerable<UniTask> processors);
    }
}