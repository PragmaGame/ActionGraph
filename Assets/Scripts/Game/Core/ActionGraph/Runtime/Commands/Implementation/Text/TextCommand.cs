using System;
using System.Text;
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
    public class TextCommand : IActionCommand
    {
        [ValueDropdown("@EditorLocalizationHelper.GetKeys()")]
        [SerializeField] private string _textKey;
        [SerializeField] private string _speakerName;

        [SerializeField] private string _debugText;
        
        private DialoguePanel _dialoguePanel;
        private LocalizationService _localizationService;

        public TextCommand()
        {
        }
        
        protected TextCommand(TextCommand data)
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
        
        public async UniTask Execute(CancellationToken token = default)
        {
            await _dialoguePanel.ShowText(_speakerName, _debugText);
        }

#if UNITY_EDITOR
        public string GetInfo(string separator)
        {
            var info = string.Empty;

            if (!string.IsNullOrEmpty(_debugText))
            {
                info = _debugText;
            }
            else if(!string.IsNullOrEmpty(_textKey))
            {
                info = EditorLocalizationHelper.GetString(_textKey);
            }
            
            return $"Text : {info}";
        }
#endif

        public virtual IActionCommand Clone() => new TextCommand(this);
    }
}