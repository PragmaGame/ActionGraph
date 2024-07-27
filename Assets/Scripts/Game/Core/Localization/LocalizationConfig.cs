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
        [SerializeField] private List<SystemLanguage> _languages;
        [SerializeField] private List<string> _keys;

        [SerializeField] private SystemLanguage _defaultLanguage = SystemLanguage.English;

        public IReadOnlyList<string> Keys => _keys;
        public IReadOnlyList<SystemLanguage> Languages => _languages;

        public bool TryGetLocalization(SystemLanguage language, out Dictionary<string, string> map)
        {
            var languageIndex = _languages.FindIndex(matchLanguage => matchLanguage == language);

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

        public string GetString(string key) => GetString(key, _defaultLanguage);

        public string GetString(string key, SystemLanguage language)
        {
            var languageIndex = _languages.FindIndex(matchLanguage => matchLanguage == language);

            return languageIndex == -1 ? string.Empty : _localizations[key][languageIndex];
        }
    }

    [Serializable]
    public class LocalizationDictionary : SerializedDictionary<string, List<string>>
    {
    }
}