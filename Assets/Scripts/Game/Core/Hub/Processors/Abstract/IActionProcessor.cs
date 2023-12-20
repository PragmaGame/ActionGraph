using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.Hub.Processors
{
    public interface IActionProcessor
    {
        public void Construct();
        public UniTask RunProcess(CancellationToken token);
    }
}