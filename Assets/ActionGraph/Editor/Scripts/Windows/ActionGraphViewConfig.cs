using Pragma.ActionGraph.Editor.Validators;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pragma.ActionGraph.Editor.Windows
{
    [CreateAssetMenu(fileName = nameof(ActionGraphViewConfig), menuName = "ActionGraph/" + nameof(ActionGraphViewConfig))]
    public class ActionGraphViewConfig : ScriptableObject
    {
        [field: SerializeField] public Color DefaultColor { get; private set; } = ParseHtmlToColor("#1D1D1E");
        [field: SerializeField] public Color ErrorColor { get; private set; }

        [field: SerializeField] public string DefaultTagNode { get; private set; } = "node";
        [field: SerializeField] public KeyValidatorParam KeyValidatorParam { get; private set; }
        
        [field: SerializeField] public StyleSheet[] StyleSheet { get; private set; }

        private static Color ParseHtmlToColor(string colorString)
        {
            ColorUtility.TryParseHtmlString(colorString, out var color);

            return color;
        }
    }
}