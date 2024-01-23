using UnityEditor;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    public static class GraphSaveUtility
    {
        public static bool IsValidPath(string path, out string validPath)
        {
            validPath = string.Empty;
            
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            
            validPath = TryConvertFullPathToLocal(path);
            return true;
        }
        
        public static void Save(NovelGraphView graph, string path)
        {
            if (!IsValidPath(path, out var validPath))
            {
                return;
            }
            
            NovelGraphData graphData = GetOrCreate<NovelGraphData>(validPath);

            graphData.graphSnapshotData = graph.SnapshotGraph();
            
            SaveAsset(graphData);
        }

        public static void Load(NovelGraphView graph, string path)
        {
            if (!IsValidPath(path, out var validPath))
            {
                return;
            }
            
            graph.DeleteAll();

            var asset = AssetDatabase.LoadAssetAtPath<NovelGraphData>(validPath);
            
            graph.LoadSnapshotData(asset.graphSnapshotData);
        }

        public static T GetOrCreate<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
            
                AssetDatabase.CreateAsset(asset, path);
            }

            return asset;
        }
        
        public static void SaveAsset(Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static string TryConvertFullPathToLocal(string path)
        {
            if (path.StartsWith(Application.dataPath)) 
            {
                path = "Assets" + path[Application.dataPath.Length..];
            }

            return path;
        }
    }
}