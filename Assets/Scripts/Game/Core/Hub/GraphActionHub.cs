using System;
using System.Collections.Generic;
using Game.Core.ActionGraph.Runtime;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub
{
    public partial class GraphActionHub : MonoBehaviour
    {
        [SerializeField] private List<GraphAction> _graphActions;

        private ActionGraphReceiver _graphReceiver;
        
        [Inject]
        private void Construct(DiContainer container, ActionGraphReceiver graphReceiver)
        {
            _graphReceiver = graphReceiver;
            
            foreach (var graphAction in _graphActions)
            {
                container.Inject(graphAction);
            }
        }

        private void OnEnable()
        {
            _graphReceiver.SwitchNodeEvent += OnSwitchNode;
        }

        private void OnDisable()
        {
            _graphReceiver.SwitchNodeEvent -= OnSwitchNode;
        }

        private void OnSwitchNode(NodeData data)
        {
            _graphActions.Find(graphAction => graphAction.Key == data.key).Run();
        }
    }
}