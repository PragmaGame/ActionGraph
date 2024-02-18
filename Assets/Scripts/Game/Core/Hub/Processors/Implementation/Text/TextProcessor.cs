using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.ActionGraph.Runtime;
using Game.Core.Localization;
using Game.Core.Panels;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub.Processors
{
    [Serializable]
    public class TextProcessor : ActionProcessor
    {
        [ValueDropdown("@EditorLocalizationHelper.GetKeys()")]
        [SerializeField] private string _textKey;
        
        [SerializeField] private string _speakerName;

        private DialoguePanel _dialoguePanel;
        private LocalizationService _localizationService;

        [Inject]
        private void Construct(DialoguePanel dialoguePanel, LocalizationService localizationService)
        {
            _dialoguePanel = dialoguePanel;
            _localizationService = localizationService;
        }
        
        public override async UniTask RunProcess(CancellationToken token = default)
        {
            await _dialoguePanel.ShowText(_speakerName, _localizationService.GetString(_textKey));
        }
    }
}