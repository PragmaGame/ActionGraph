using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime.Obsolete
{
    [CreateAssetMenu(fileName = nameof(NodeMetaDataParser), menuName = "Parsers/" + nameof(NodeMetaDataParser))]
    public partial class NodeMetaDataParser : ScriptableObject
    {
        [SerializeField] private string _keyPrefix = "//";
        [SerializeField] private NodeKeyMap _keysMap;

        public Dictionary<KeyMetaDataType, List<string>> Parse(string data)
        {
            var parseData = new Dictionary<KeyMetaDataType, List<string>>();
            
            var splitData = data.Split("\n");
            var lastKeyType = KeyMetaDataType.None;
            
            foreach (var dataItem in splitData)
            {
                if (dataItem.StartsWith(_keyPrefix))
                {
                    var key = dataItem[_keyPrefix.Length..];

                    var keyType = _keysMap[key];

                    parseData.Add(keyType, new List<string>());
                    
                    lastKeyType = keyType;
                }
                else
                {
                    parseData[lastKeyType].Add(dataItem);
                }
            }

            return parseData;
        }
        
        [Serializable]
        private class NodeKeyMap : SerializedDictionary<string, KeyMetaDataType> { }
    }
}