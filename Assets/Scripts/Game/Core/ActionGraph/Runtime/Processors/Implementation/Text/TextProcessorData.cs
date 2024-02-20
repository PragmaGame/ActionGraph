using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class TextProcessorData : IActionProcessorData
    {
        [ValueDropdown("@EditorLocalizationHelper.GetKeys()")]
        public string textKey;
        public string speakerName;
    }
}