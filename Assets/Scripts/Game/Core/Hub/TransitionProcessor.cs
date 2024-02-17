using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core.ActionGraph.Runtime;
using Game.Core.Localization;
using Game.Core.Panels;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub
{
    [Serializable]
    public class TransitionProcessor
    {
        [SerializeField] private List<TransitionParam> _transitionParams;

        private TransitionPanel _transitionPanel;
        private ActionGraphReceiver _actionGraphReceiver;
        private LocalizationService _localizationService;
        
        [Inject]
        private void Construct(TransitionPanel transitionPanel, 
            ActionGraphReceiver actionGraphReceiver,
            LocalizationService localizationService)
        {
            _transitionPanel = transitionPanel;
            _actionGraphReceiver = actionGraphReceiver;
            _localizationService = localizationService;
        }
        
        public async UniTask<int> SelectTransition(NodeData data)
        {
            var result = await _transitionPanel.WaitSelection(_transitionParams);

            return string.IsNullOrEmpty(result) ? 0 : _transitionParams.FindIndex(x => x.key == result);
        }
    }
}