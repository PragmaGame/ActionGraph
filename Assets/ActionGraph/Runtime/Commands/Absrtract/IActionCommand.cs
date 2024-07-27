using System.Threading;
using Cysharp.Threading.Tasks;

namespace Pragma.ActionGraph.Runtime.Commands
{
    public interface IActionCommand
    {
        public UniTask Execute(CancellationToken token = default);
        public IActionCommand Clone();

#if UNITY_EDITOR
        public string GetInfo(string separator = "\n") => string.Empty;
#endif
    }
}