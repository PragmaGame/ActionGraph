#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.Localization
{
    public partial class LocalizationConfig
    {
        [SerializeField] private TextAsset _csv;
        [SerializeField] private string _rootTable = "Key";
        
        [Button]
        private void Parse()
        {
            ICsvParser csvParser = new CsvParser();

            var parseResult = csvParser.Parse(_csv.text, Delimiter.Comma);

            _languages = parseResult[_rootTable].ToList();

            parseResult.Remove(_rootTable);
            
            _keys = new List<string>();

            for (var i = 0; i < parseResult.Count; i++)
            {
                _keys.Add(parseResult.ElementAt(i).Key);
            }

            _localizations.Replace(parseResult);
        }
    }
}

#endif