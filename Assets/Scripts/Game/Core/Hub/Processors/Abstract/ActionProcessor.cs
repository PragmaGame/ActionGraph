using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace Game.Core.Hub.Processors
{
    public abstract class ActionProcessor : IActionProcessor
    {
        [Button(ButtonStyle.FoldoutButton)]
        public abstract UniTask RunProcess(CancellationToken token = default);
    }
}