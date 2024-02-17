using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Panels
{
    public class TransitionPanel : MonoBehaviour
    {
        [SerializeField] private Button _mainButton;
        
        [SerializeField] private List<TransitionItem> _items;

        private string _result;
        private bool _isNotifyClick = true;

        public event Action ClickEvent; 

        private void OnEnable()
        {
            _mainButton.onClick.AddListener(OnClickMainButton);
        }

        private void OnDisable()
        {
            _mainButton.onClick.RemoveListener(OnClickMainButton);
        }

        private void OnClickMainButton()
        {
            _result = null;

            if (_isNotifyClick)
            {
                ClickEvent?.Invoke();
            }
        }

        public async UniTask<string> WaitSelection(List<TransitionParam> param)
        {
            _isNotifyClick = false;
            
            if (param.Count > 1)
            {
                Setup(param);
            }

            await UniTask.WaitUntilValueChanged(this,_ => _result);

            _isNotifyClick = true;
            
            return _result;
        }

        private void Setup(List<TransitionParam> param)
        {
            foreach (var selection in _items)
            {
                var suitableParam = param.Find(x => x.transitionPosition == selection.Position);

                if (suitableParam == null)
                {
                    continue;
                }
                
                selection.Setup(suitableParam);
                selection.ClickTransitionEvent += OnClickTransition;
                
                selection.gameObject.SetActive(true);
            }
        }

        private void OnClickTransition(TransitionItem selectItem)
        {
            foreach (var selection in _items)
            {
                selection.ClickTransitionEvent -= OnClickTransition;
                selection.gameObject.SetActive(false);
            }

            _result = selectItem.Key;
        }
    }
}