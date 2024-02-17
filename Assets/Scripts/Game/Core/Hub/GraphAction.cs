using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core.ActionGraph.Runtime;
using Game.Core.Hub.ProcessRunners;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub
{
    public class GraphAction : MonoBehaviour
    {
        [ValueDropdown("@ActionGraphDataEditorHelper.GetNodesGraphsKeys()", NumberOfItemsBeforeEnablingSearch = 1)] 
        [SerializeField] private string _key;
        
        [SerializeField] private List<ProcessorHub> _processorHubs;
        
        [SerializeField] private TransitionProcessor _transitionProcessor;

        private SequenceProcessRunner _runner;
        private CancellationTokenSource _cancellationTokenSource;

        private ActionGraphReceiver _actionGraphReceiver;

        public void SetKey(string key)
        {
            _key = key;
        }

        [Inject]
        private void Construct(DiContainer container, ActionGraphReceiver actionGraphReceiver)
        {
            _actionGraphReceiver = actionGraphReceiver;

            foreach (var processorHub in _processorHubs)
            {
                container.Inject(processorHub);
            }
            
            container.Inject(_transitionProcessor);
        }

        private void Awake()
        {
            _runner = new SequenceProcessRunner();
        }

        private void OnEnable()
        {
            _actionGraphReceiver.SubscribeToSwitchConcreteNode(_key, Run);
        }

        private void OnDisable()
        {
            _actionGraphReceiver.UnsubscribeToSwitchConcreteNode(_key, Run);
        }

        [HideInEditorMode, Button(ButtonStyle.FoldoutButton)]
        public async void Run()
        {
            var data = _actionGraphReceiver.CurrentNode;
            
            _cancellationTokenSource = new CancellationTokenSource();

            await _runner.RunProcess(_processorHubs.Select(x => x.RunProcess(data, _cancellationTokenSource.Token))).SuppressCancellationThrow();

            var transition = await _transitionProcessor.SelectTransition(data);
            
            _actionGraphReceiver.SwitchToNextNode(transition);
        }

        [HideInEditorMode, Button(ButtonStyle.FoldoutButton)]
        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}