using System;
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
    [Serializable]
    public class GraphAction
    {
        [ValueDropdown("@ActionGraphDataEditorHelper.GetNodesGraphsKeys()", NumberOfItemsBeforeEnablingSearch = 1)] 
        [SerializeField] private string _key;
        
        [SerializeField] private List<ProcessorHub> _processorHubs;

        private SequenceProcessRunner _runner;
        private CancellationTokenSource _cancellationTokenSource;

        public string Key => _key;
        
        public GraphAction()
        {
        }

        public GraphAction(string key)
        {
            _key = key;
        }

        [Inject]
        private void Construct(DiContainer container)
        {
            _runner = new SequenceProcessRunner();

            foreach (var processorHub in _processorHubs)
            {
                container.Inject(processorHub);
            }
        }
        
        [HideInEditorMode, Button(ButtonStyle.FoldoutButton)]
        public async void Run()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            if(await _runner.RunProcess(_processorHubs.Select(x => x.RunProcess(_cancellationTokenSource.Token))).SuppressCancellationThrow())
            {
                Debug.Log("Canceled");
            }
        }

        [HideInEditorMode, Button(ButtonStyle.FoldoutButton)]
        public void Stop()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}