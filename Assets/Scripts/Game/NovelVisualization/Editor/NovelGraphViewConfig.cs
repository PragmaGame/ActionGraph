using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    [CreateAssetMenu(fileName = nameof(NovelGraphViewConfig), menuName = "NovelGraph/" + nameof(NovelGraphViewConfig))]
    public class NovelGraphViewConfig : ScriptableObject
    {
        [field: SerializeField] public Color DefaultColor { get; private set; } = ParseHtmlToColor("#1D1D1E");
        [field: SerializeField] public Color ErrorColor { get; private set; }

        private static Color ParseHtmlToColor(string colorString)
        {
            ColorUtility.TryParseHtmlString(colorString, out var color);

            return color;
        }
    }
}