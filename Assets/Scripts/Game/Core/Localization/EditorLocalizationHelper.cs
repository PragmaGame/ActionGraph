#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using Common;

namespace Game.Core.Localization
{
    public static class EditorLocalizationHelper
    {
        public static IEnumerable<string> GetKeys()
        {
            var configs = AssetDatabaseExtensions.GetAssets<LocalizationConfig>();

            return configs.SelectMany(config => config.Keys);
        }
        
        public static string GetString(string key)
        {
            var config = AssetDatabaseExtensions.GetAsset<LocalizationConfig>();

            return config.GetString(key);
        }
    }
}

#endif