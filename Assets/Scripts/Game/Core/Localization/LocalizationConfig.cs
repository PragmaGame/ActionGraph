using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace Game.Core.Localization
{
    [CreateAssetMenu(fileName = nameof(LocalizationConfig), menuName = "Configs/Services/" + nameof(LocalizationConfig))]
    public partial class LocalizationConfig : ScriptableObject
    {
        [SerializeField] private LocalizationDictionary _localizations;
        [SerializeField] private List<string> _languages;
        [SerializeField] private List<string> _keys;

        [SerializeField] private string _defaultLanguage = "English";

        public IReadOnlyList<string> Keys => _keys;
        public IReadOnlyList<string> Languages => _languages;

        public bool TryGetLocalization(string language, out Dictionary<string, string> map)
        {
            var languageIndex = _languages.FindIndex(matchLanguage => matchLanguage.Contains(language));

            if (languageIndex != -1)
            {
                map = _localizations.ToDictionary(pair => pair.Key, pair => pair.Value[languageIndex]);
                return true;
            }

            map = null;
            return false;
        }

        public Dictionary<string, string> GetDefaultLocalization()
        {
            TryGetLocalization(_defaultLanguage, out var map);

            return map;
        }
    }
    
    [Serializable]
    public class LocalizationDictionary : SerializedDictionary<string, List<string>>
    {
    }
}