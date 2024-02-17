using System;
using UnityEngine;

namespace Game.Core.Panels
{
    [Serializable]
    public class TransitionParam
    {
        public TransitionPosition transitionPosition = TransitionPosition.Center;
        public Sprite background;
        public string title;
        public string key;
    }

    public enum TransitionPosition
    {
        Center = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
    }
}