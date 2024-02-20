using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core.ActionGraph.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Panels
{
    public class TransitionPanel : MonoBehaviour
    {
        [SerializeField] private Button _mainButton;
        
        [SerializeField] private List<TransitionItem> _items;

        private TransitionData _result;
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

        public async UniTask<TransitionData> WaitSelection(List<TransitionData> data)
        {
            _isNotifyClick = false;
            
            if (data.Count > 1)
            {
                Setup(data);
            }

            await UniTask.WaitUntilValueChanged(this,_ => _result);

            _isNotifyClick = true;
            
            return _result;
        }

        private void Setup(List<TransitionData> data)
        {
            foreach (var selection in _items)
            {
                var suitableParam = data.Find(x => x.transitionPosition == selection.Position);

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

            _result = selectItem.Data;
        }
    }
}