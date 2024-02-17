using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.ActionGraph.Runtime;

namespace Game.Core.Hub.Processors
{
    public interface IActionProcessor
    {
        public UniTask RunProcess(NodeData data, CancellationToken token = default);
    }
}