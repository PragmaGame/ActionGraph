using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pragma.ActionGraph.Runtime.Commands
{
    [Serializable]
    public class TextCommand : IActionCommand
    {
        [ValueDropdown("@EditorLocalizationHelper.GetKeys()")]
        [SerializeField] private string _textKey;
        [SerializeField] private string _speakerName;

        [SerializeField] private string _debugText;
        
        // private DialoguePanel _dialoguePanel;
        // private LocalizationService _localizationService;

        public TextCommand()
        {
        }
        
        protected TextCommand(TextCommand data)
        {
            _textKey = string.Copy(data._textKey);
            _speakerName = string.Copy(data._speakerName);
            _debugText = string.Copy(data._debugText);
        }
        
        // [Inject]
        // public void Construct(DialoguePanel dialoguePanel, LocalizationService localizationService)
        // {
        //     _dialoguePanel = dialoguePanel;
        //     _localizationService = localizationService;
        // }
        
        public async UniTask Execute(CancellationToken token = default)
        {
            // var text = !string.IsNullOrEmpty(_debugText) ? _debugText : _localizationService.GetString(_textKey);
            //
            // await _dialoguePanel.ShowText(_speakerName, text);

            return;
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
                //info = EditorLocalizationHelper.GetString(_textKey);
            }
            
            return $"Text : {info}";
        }
#endif

        public virtual IActionCommand Clone() => new TextCommand(this);
    }
}