using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Common.Editor
{
    public static class AssetDatabaseExtensions
    {
        public static IEnumerable<T> GetAssets<T>(string t = "ScriptableObject") where T : Object
        {
            var assets = AssetDatabase.FindAssets(typeof(T).Name + " t:" + t);

            if (assets.Length < 1) return default;

            return assets.Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>);
        }

        public static T GetAsset<T>(string t = "ScriptableObject") where T : Object
        {
            var guids = AssetDatabase.FindAssets(typeof(T).Name + " t:" + t);

            if (guids.Length < 1) return null;
			
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }
    }
}