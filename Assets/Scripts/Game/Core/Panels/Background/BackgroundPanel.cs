using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Game.Core.Panels
{
    public class BackgroundPanel : MonoBehaviour
    {
        [SerializeField] private Image _background;
        
        public void SwitchBackground(Sprite sprite)
        {
            _background.sprite = sprite;
        }
    }
}