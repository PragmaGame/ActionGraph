using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Core.Hub.ProcessRunners
{
    public interface IProcessRunner
    {
        public UniTask RunProcess(IEnumerable<UniTask> processors);
    }
}