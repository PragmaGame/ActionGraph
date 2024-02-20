using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEditorInternal;

namespace Game.Core.ActionGraph.Runtime
{
    public abstract class ActionProcessor<TData> : IActionProcessor where TData : IActionProcessorData
    {
        [HideInEditorMode, Button(ButtonStyle.FoldoutButton)]
        public abstract UniTask RunProcess(TData data, CancellationToken token = default);

        public Type DataType => typeof(TData);
        
        public bool IsCanProcess(object data) => data is TData;

        [HideInEditorMode, Button(ButtonStyle.FoldoutButton)]
        public UniTask RunProcess(object data, CancellationToken token = default)
        {
            if (data is TData covertData)
            {
                return RunProcess(covertData, token);
            }
            
            return UniTask.CompletedTask;
        }
    }
}