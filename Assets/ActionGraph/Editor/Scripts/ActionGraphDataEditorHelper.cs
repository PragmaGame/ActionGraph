using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Pragma.ActionGraph.Runtime.Save;
using UnityEditor;

namespace Pragma.ActionGraph.Editor
{
    [UsedImplicitly]
    public static class ActionGraphDataEditorHelper
    {
        [UsedImplicitly]
        public static IEnumerable GetNodesGraphsKeys()
        {
            return GetNodes().Select(x => x.Key);
        }
        
        [UsedImplicitly]
        public static IEnumerable<ActionNodeData> GetNodes()
        {
            return AssetDatabase.FindAssets($"t:{typeof(ActionGraphData)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ActionGraphData>)
                .SelectMany(x => x.Nodes);
        }

        [UsedImplicitly]
        public static IEnumerable GetGroupsKeys(ActionGraphData data)
        {
            if (data == null)
            {
                return default;
            }
            
            return data.Groups.Select(x => x.Key);
        }
    }
}