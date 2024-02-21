using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Panels;
using UnityEngine;
using Zenject;

namespace Game.Core.ActionGraph.Runtime.Background
{
    [Serializable]
    public class BackgroundSwitchProcessor : IActionProcessor
    {
        [SerializeField] private Sprite _sprite;

        private BackgroundPanel _panel;
        
        public BackgroundSwitchProcessor()
        {
            
        }

        protected BackgroundSwitchProcessor(BackgroundSwitchProcessor data)
        {
            _sprite = data._sprite;
        }

        [Inject]
        public void Construct(BackgroundPanel panel)
        {
            _panel = panel;
        }
        
        public UniTask RunProcess(CancellationToken token = default)
        {
            _panel.SwitchBackground(_sprite);
            
            return UniTask.CompletedTask;
        }

        public IActionProcessor Clone() => new BackgroundSwitchProcessor(this);
    }
}