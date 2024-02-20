using System;
using Game.Core.ActionGraph.Runtime;
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

        public TransitionData Data { get; private set; }

        public void Setup(TransitionData data)
        {
            _background.sprite = data.background;
            _title.text = data.title;

            Data = data;
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