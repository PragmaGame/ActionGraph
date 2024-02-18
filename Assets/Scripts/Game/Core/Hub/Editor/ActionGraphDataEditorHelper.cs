using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core.ActionGraph.Runtime;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Hub.Editor
{
    [UsedImplicitly]
    public static class ActionGraphDataEditorHelper
    {
        private static ActionGraphData[] _data;
        
        // [UsedImplicitly]
        // public static IEnumerable GetNodesGraphsKeys()
        // {
        //     return GetNodes().Select(x => x.key);
        // }
        //
        // [UsedImplicitly]
        // public static IEnumerable<NodeData> GetNodes()
        // {
        //     Debug.Log("Get Nodes");
        //     
        //     return AssetDatabase.FindAssets($"t:{typeof(ActionGraphData)}")
        //         .Select(AssetDatabase.GUIDToAssetPath)
        //         .Select(AssetDatabase.LoadAssetAtPath<ActionGraphData>)
        //         .SelectMany(x => x.GetOriginalData().nodes);
        // }

        // [UsedImplicitly]
        // public static IEnumerable GetGroupsKeys(ActionGraphData data)
        // {
        //     if (data == null)
        //     {
        //         return default;
        //     }
        //     
        //     return data.GetSnapshotData().groups.Select(x => x.key);
        // }
    }
}