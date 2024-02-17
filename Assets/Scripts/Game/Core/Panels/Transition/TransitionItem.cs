using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Panels
{
    public class TransitionItem : MonoBehaviour
    {
        [SerializeField] private TransitionPosition _position;
        
        [SerializeField] private Button _button;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _title;

        public event Action<TransitionItem> ClickTransitionEvent;

        public TransitionPosition Position => _position;
        
        public string Key { get; private set; }

        public void Setup(TransitionParam param)
        {
            _background.sprite = param.background;
            _title.text = param.title;

            Key = param.key;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickTransition);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickTransition);
        }

        private void OnClickTransition()
        {
            ClickTransitionEvent?.Invoke(this);
        }
    }
}