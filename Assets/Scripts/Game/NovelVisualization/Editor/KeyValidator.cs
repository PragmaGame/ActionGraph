using System.Text.RegularExpressions;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    public class KeyValidator
    {
        private KeyValidatorParam _param;

        public KeyValidator(KeyValidatorParam param)
        {
            _param = param;
        }

        public bool TryValidate(string value, out string result)
        {
            result = string.Empty;
            
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            
            result = value.ToLower();
            
            foreach (var pattern in _param.Patterns)
            {
                result = Regex.Replace(result, pattern.value1, pattern.value2);
            }
            
            return true;
        }
    }
}