#if UNITY_EDITOR

using Game.Core.ActionGraph.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.Hub
{
    public partial class GraphActionBuilder
    {
        [Space(25)]
        [InfoBox("Editor Only")]
        [SerializeField] private ActionGraphData _data;

        [SerializeField] private bool _isUseGroupKey;
        
        [ShowIf("_isUseGroupKey")]
        [ValueDropdown("@ActionGraphDataEditorHelper.GetGroupsKeys(_data)")] 
        [SerializeField] private string _groupKey;
        
        [HideInPlayMode, Button]
        public void FillGraphActions()
        {
            if (_isUseGroupKey)
            {
                var group = _data.GetSnapshotData().groups.Find(x => x.key == _groupKey);
            
                foreach (var key in group.ownedNodesKeys)
                {
                    var graphAction = gameObject.AddComponent<GraphAction>();
                    graphAction.SetKey(key);
                }
            }
            else
            {
                foreach (var nodeData in _data.GetSnapshotData().nodes)
                {
                    var graphAction = gameObject.AddComponent<GraphAction>();
                    graphAction.SetKey(nodeData.key);
                }
            }
        }
    }
}

#endif