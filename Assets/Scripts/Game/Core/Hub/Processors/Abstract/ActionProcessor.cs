using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.ActionGraph.Runtime;
using Sirenix.OdinInspector;

namespace Game.Core.Hub.Processors
{
    public abstract class ActionProcessor : IActionProcessor
    {
        [HideInEditorMode, Button(ButtonStyle.FoldoutButton)]
        public abstract UniTask RunProcess(NodeData data, CancellationToken token = default);
    }
}