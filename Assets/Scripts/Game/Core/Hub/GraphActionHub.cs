using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Hub
{
    public class GraphActionHub : MonoBehaviour
    {
        [SerializeField] private List<GraphAction> _actionHubs;

        private void Awake()
        {
            foreach (var actionHub in _actionHubs)
            {
                actionHub.Construct();
            }
        }
    }
}