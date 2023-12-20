using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.Graph
{
    public partial class NodeMetaDataParser
    {
        [TextArea(10, 20)]
        [SerializeField] private string _debug;

        [TextArea(10, 20)]
        [ShowInInspector, ReadOnly] private string _debugInfo;

        [Button(ButtonStyle.FoldoutButton)]
        private void Parse()
        {
            var data = Parse(_debug);

            _debugInfo = string.Empty;
            
            foreach (var pair in data)
            {
                _debugInfo += "Key : " + pair.Key + "\n";

                foreach (var value in pair.Value)
                {
                    _debugInfo += "Value : " + value + "\n";
                }
            }
        }
    }
}