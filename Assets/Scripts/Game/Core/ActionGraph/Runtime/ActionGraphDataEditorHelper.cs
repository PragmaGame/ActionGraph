using System.Collections;
using System.Linq;
using UnityEditor;

namespace Game.Core.ActionGraph.Runtime
{
    public static class ActionGraphDataEditorHelper
    {
        public static IEnumerable GetNodesGraphsKeys()
        {
            return AssetDatabase.FindAssets($"t:{typeof(ActionGraphData)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ActionGraphData>)
                .SelectMany(x => x.GetSnapshotData().nodes)
                .Select(x => x.key);
        }

        public static IEnumerable GetGroupsKeys(ActionGraphData data)
        {
            if (data == null)
            {
                return default;
            }
            
            return data.GetSnapshotData().groups.Select(x => x.key);
        }
    }
}