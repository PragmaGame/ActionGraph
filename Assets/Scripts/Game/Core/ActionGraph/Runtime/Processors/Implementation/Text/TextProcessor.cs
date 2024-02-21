using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Localization;
using Game.Core.Panels;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class TextProcessor : IActionProcessor
    {
        [ValueDropdown("@EditorLocalizationHelper.GetKeys()")]
        [SerializeField] private string _textKey;
        [SerializeField] private string _speakerName;

        [SerializeField] private string _debugText;
        
        private DialoguePanel _dialoguePanel;
        private LocalizationService _localizationService;

        public TextProcessor()
        {
        }
        
        protected TextProcessor(TextProcessor data)
        {
            _textKey = string.Copy(data._textKey);
            _speakerName = string.Copy(data._speakerName);
            _debugText = string.Copy(data._debugText);
        }
        
        [Inject]
        public void Construct(DialoguePanel dialoguePanel, LocalizationService localizationService)
        {
            _dialoguePanel = dialoguePanel;
            _localizationService = localizationService;
        }
        
        public async UniTask RunProcess(CancellationToken token = default)
        {
            await _dialoguePanel.ShowText(_speakerName, _debugText);
        }

        public IActionProcessor Clone() => new TextProcessor(this);
    }
}