using System.Threading;
using Cysharp.Threading.Tasks;

namespace ActionGraph.Runtime.Commands.Absrtract
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