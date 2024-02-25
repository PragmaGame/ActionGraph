using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Localization
{
    public class LocalizationService
    {
        private LocalizationConfig _config;
        
        private Dictionary<string, string> _map;
        private Dictionary<string, string> _mapDefault;

        public LocalizationService(LocalizationConfig config)
        {
            _config = config;
            
            LoadLocalization();
        }

        private void LoadLocalization()
        {
            _mapDefault = _config.GetDefaultLocalization();
            
#if !UNITY_EDITOR
            if (!TrySetLocalization(Application.systemLanguage))
            {
                _map = _mapDefault;
            }
#else
            _map = _mapDefault;
#endif
        }
        
        public string GetString(string key)
        {
            if (_map.TryGetValue(key, out var value))
            {
                return value;
            }
            
            return _mapDefault[key];
        }

        public bool TrySetLocalization(SystemLanguage language)
        {
            if(_config.TryGetLocalization(language, out var map))
            {
                _map = map;
                return true;
            }
            
            return false;
        }
    }
}