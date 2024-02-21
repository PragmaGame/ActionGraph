using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    public class ActionGraphToolBarView
    {
        private ActionGraphToolBarViewConfig _config;

        private TextField _searchTextField;
        
        private Button _miniMapButton;

        private string _lastOpenGraphPath;
        
        public event Action<string> ChangeSearchFieldEvent;
        public event Action ClickMiniMapButtonEvent;
        
        public Toolbar Toolbar { get; private set; }

        public ActionGraphToolBarView(ActionGraphToolBarViewConfig config)
        {
            _config = config;
            
            Toolbar = new Toolbar();

            _searchTextField = new TextField()
            {
                label = "Search Node:",
            };

            _searchTextField.RegisterValueChangedCallback(OnChangeSearchTextField);
            
            Toolbar.Add(_searchTextField);

            _miniMapButton = new Button(OnClickMiniMapButton)
            {
                text = "Mini Map",
            };
            
            Toolbar.Add(_miniMapButton);
            
            Toolbar.styleSheets.Add(_config.StyleSheet);
        }

        private void OnClickMiniMapButton()
        {
            ClickMiniMapButtonEvent?.Invoke();
        }

        private void OnChangeSearchTextField(ChangeEvent<string> callback)
        {
            ChangeSearchFieldEvent?.Invoke(callback.newValue);
        }
    }
}  