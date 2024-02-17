using Game.Core.ActionGraph.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game.Core.Hub
{
    public class DebugActionInvoker : MonoBehaviour
    {
        [ValueDropdown("@ActionGraphDataEditorHelper.GetNodesGraphsKeys()", NumberOfItemsBeforeEnablingSearch = 1)] 
        [SerializeField] private string _debugNode;
        
        private ActionGraphReceiver _receiver;
        
        [Inject]
        private void Construct(ActionGraphReceiver receiver)
        {
            _receiver = receiver;
        }

        [Button(ButtonStyle.FoldoutButton)]
        public void Run()
        {
            _receiver.SwitchToNode(_debugNode);
        }
    }
}