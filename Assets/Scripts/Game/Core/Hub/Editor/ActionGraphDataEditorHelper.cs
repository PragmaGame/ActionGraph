using System.Collections;
using System.Linq;
using Game.Core.ActionGraph.Runtime;
using JetBrains.Annotations;
using UnityEditor;

namespace Game.Core.Hub.Editor
{
    [UsedImplicitly]
    public static class ActionGraphDataEditorHelper
    {
        [UsedImplicitly]
        public static IEnumerable GetNodesGraphsKeys()
        {
            return AssetDatabase.FindAssets($"t:{typeof(ActionGraphData)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ActionGraphData>)
                .SelectMany(x => x.GetSnapshotData().nodes)
                .Select(x => x.key);
        }

        [UsedImplicitly]
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