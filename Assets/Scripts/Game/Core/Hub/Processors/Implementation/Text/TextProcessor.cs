using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Dialogue;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub.Processors
{
    [Serializable]
    public class TextProcessor : ActionProcessor
    {
        [SerializeField] private string _speakerName;
        [SerializeField] private string _text;

        private DialoguePanel _dialoguePanel;

        [Inject]
        private void Construct(DialoguePanel dialoguePanel)
        {
            _dialoguePanel = dialoguePanel;
        }
        
        public override async UniTask RunProcess(CancellationToken token = default)
        {
            await _dialoguePanel.ShowText(_speakerName, _text);
        }
    }
}