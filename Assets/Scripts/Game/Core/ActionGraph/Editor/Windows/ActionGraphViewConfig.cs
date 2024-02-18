using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    [CreateAssetMenu(fileName = nameof(ActionGraphViewConfig), menuName = "ActionGraph/" + nameof(ActionGraphViewConfig))]
    public class ActionGraphViewConfig : ScriptableObject
    {
        [field: SerializeField] public Color DefaultColor { get; private set; } = ParseHtmlToColor("#1D1D1E");
        [field: SerializeField] public Color ErrorColor { get; private set; }

        [field: SerializeField] public string DefaultKeyNode { get; private set; } = "default-key";
        [field: SerializeField] public KeyValidatorParam KeyValidatorParam { get; private set; }
        
        [field: SerializeField] public StyleSheet[] StyleSheet { get; private set; }
        
        [field: SerializeField] public string DefaultNodePath{ get; private set; }

        private static Color ParseHtmlToColor(string colorString)
        {
            ColorUtility.TryParseHtmlString(colorString, out var color);

            return color;
        }
    }
}