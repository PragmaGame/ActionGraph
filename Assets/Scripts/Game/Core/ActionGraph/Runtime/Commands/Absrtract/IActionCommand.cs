using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.ActionGraph.Runtime
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