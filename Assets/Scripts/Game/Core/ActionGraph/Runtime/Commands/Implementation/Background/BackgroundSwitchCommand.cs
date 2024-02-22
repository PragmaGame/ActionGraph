using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Panels;
using UnityEngine;
using Zenject;

namespace Game.Core.ActionGraph.Runtime.Background
{
    [Serializable]
    public class BackgroundSwitchCommand : IActionCommand
    {
        [SerializeField] private Sprite _sprite;

        private BackgroundPanel _panel;
        
        public BackgroundSwitchCommand()
        {
            
        }

        protected BackgroundSwitchCommand(BackgroundSwitchCommand data)
        {
            _sprite = data._sprite;
        }

        [Inject]
        public void Construct(BackgroundPanel panel)
        {
            _panel = panel;
        }
        
        public UniTask Execute(CancellationToken token = default)
        {
            _panel.SwitchBackground(_sprite);
            
            return UniTask.CompletedTask;
        }

        public IActionCommand Clone() => new BackgroundSwitchCommand(this);
    }
}