using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class TransitionData
    {
        [ReadOnly] public string nodeKey;
        
        public TransitionPosition transitionPosition = TransitionPosition.Center;
        public Sprite background;
        public string title;
    }
}