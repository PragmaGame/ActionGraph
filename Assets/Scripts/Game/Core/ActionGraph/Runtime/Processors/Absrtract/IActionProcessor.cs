using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.ActionGraph.Runtime
{
    public interface IActionProcessor
    {
        public UniTask RunProcess(CancellationToken token = default);
        public IActionProcessor Clone();
    }
}