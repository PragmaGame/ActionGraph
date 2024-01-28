using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    [CreateAssetMenu(fileName = nameof(ActionGraphToolBarViewConfig), menuName = "ActionGraph/" + nameof(ActionGraphToolBarViewConfig))]
    public class ActionGraphToolBarViewConfig : ScriptableObject
    {
        [field: SerializeField] public StyleSheet StyleSheet { get; private set; }
    }
}