using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core.Panels;
using Zenject;

namespace Game.Core.ActionGraph.Runtime
{
    public class ActionNodeSelector
    {
        private TransitionPanel _transitionPanel;

        [Inject]
        private void Construct(TransitionPanel transitionPanel)
        {
            _transitionPanel = transitionPanel;
        }
        
        public async UniTask<TransitionData> SelectNode(List<TransitionData> dates)
        {
            return await _transitionPanel.WaitSelection(dates);
        }
    }
}