using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.Localization;
using Game.Core.Panels;

namespace Game.Core.ActionGraph.Runtime
{
    public class TextProcessor : ActionProcessor<TextProcessorData>
    {
        private DialoguePanel _dialoguePanel;
        private LocalizationService _localizationService;

        public TextProcessor(DialoguePanel dialoguePanel, LocalizationService localizationService)
        {
            _dialoguePanel = dialoguePanel;
            _localizationService = localizationService;
        }
        
        public override async UniTask RunProcess(TextProcessorData data, CancellationToken token = default)
        {
            await _dialoguePanel.ShowText(data.speakerName, _localizationService.GetString(data.textKey));
        }
    }
}