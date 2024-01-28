using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub
{
    public partial class GraphActionHub : MonoBehaviour
    {
        [SerializeField] private List<GraphAction> _graphActions;

        [Inject]
        private void Construct(DiContainer container)
        {
            foreach (var graphAction in _graphActions)
            {
                container.Inject(graphAction);
            }
        }
    }
}