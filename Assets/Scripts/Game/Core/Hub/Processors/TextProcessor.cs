using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core.Hub.Processors
{
    [Serializable]
    public class TextProcessor : ActionProcessor
    {
        [SerializeField] private string text;
        [SerializeField] private float _delayer;

        public override void Construct()
        {
            
        }

        public override async UniTask RunProcess(CancellationToken token)
        {
            Debug.Log("Start : " + text);
            await UniTask.Delay(TimeSpan.FromSeconds(_delayer), cancellationToken: token);
            Debug.Log("End : " + text);
        }
    }
}