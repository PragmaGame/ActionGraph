using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Pragma.ActionGraph.Runtime
{
    public class DebugActionInvoker : MonoBehaviour
    {
        [ValueDropdown("@ActionGraphDataEditorHelper.GetNodesGraphsKeys()", NumberOfItemsBeforeEnablingSearch = 1)] 
        [SerializeField] private string _debugNode;
        
        private ActionGraphInvoker _invoker;
        
        [Inject]
        private void Construct(ActionGraphInvoker invoker)
        {
            _invoker = invoker;
        }

        [Button(ButtonStyle.FoldoutButton)]
        public void Invoke()
        {
            _invoker.SwitchToNode(_debugNode);
        }
        
        [Button(ButtonStyle.FoldoutButton)]
        public void Cancel()
        {
            _invoker.CancelInvoke();
        }
    }
}