using System.Collections.Generic;
using System.Linq;
using Common.Editor;

namespace Game.Core.Localization.Editor
{
    public static class EditorLocalizationHelper
    {
        public static IEnumerable<string> GetKeys()
        {
            var configs = AssetDatabaseExtensions.GetAssets<LocalizationConfig>();

            return configs.SelectMany(config => config.Keys);
        }
    }
}