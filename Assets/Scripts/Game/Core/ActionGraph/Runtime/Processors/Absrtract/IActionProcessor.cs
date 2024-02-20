using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Game.Core.ActionGraph.Runtime
{
    public interface IActionProcessor
    {
        public Type DataType { get; }
        public UniTask RunProcess(object data, CancellationToken token = default);
    }
}