using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.Hub.Processors
{
    public interface IActionProcessor
    {
        public UniTask RunProcess(CancellationToken token = default);
    }
}