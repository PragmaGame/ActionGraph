using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Localization
{
    public class LocalizationService
    {
        private LocalizationConfig _config;
        
        private Dictionary<string, string> _map;

        public LocalizationService(LocalizationConfig config)
        {
            _config = config;
            
            SetLocalization();
        }

        public string GetString(string key)
        {
            return _map[key];
        }

        public void SetLocalization(string language = null)
        {
            if (string.IsNullOrEmpty(language))
            {
                language = Enum.GetName(typeof(SystemLanguage), Application.systemLanguage);
            }

            _map = _config.TryGetLocalization(language, out var map) ? map : _config.GetDefaultLocalization();
        }
    }
}