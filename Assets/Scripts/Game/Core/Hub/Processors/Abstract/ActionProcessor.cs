using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.Hub.Processors
{
    public abstract class ActionProcessor : IActionProcessor
    {
        public abstract void Construct();

        public abstract UniTask RunProcess(CancellationToken token);
    }
}