using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Pragma.ActionGraph.Editor.Windows
{
    public class ActionGraphToolBarView
    {
        private ActionGraphToolBarViewConfig _config;

        private TextField _searchTextField;
        
        private Button _miniMapButton;
        private Button _saveButton;

        private string _lastOpenGraphPath;
        
        public event Action<string> ChangeSearchFieldEvent;
        public event Action ClickMiniMapButtonEvent;
        public event Action ClickSaveButtonEvent;
        
        public Toolbar Toolbar { get; private set; }

        public ActionGraphToolBarView(ActionGraphToolBarViewConfig config)
        {
            _config = config;
            
            Toolbar = new Toolbar();

            _searchTextField = new TextField()
            {
                label = "Search : ",
            };

            _searchTextField.RegisterValueChangedCallback(OnChangeSearchTextField);
            
            Toolbar.Add(_searchTextField);
            
            _miniMapButton = new Button(OnClickMiniMapButton)
            {
                text = "Mini Map",
            };
            
            Toolbar.Add(_miniMapButton);

            _saveButton = new Button(OnClickSaveButton)
            {
                text = "Save",
            };
            
            Toolbar.Add(_saveButton);
            
            Toolbar.styleSheets.Add(_config.StyleSheet);
        }

        private void OnClickMiniMapButton()
        {
            ClickMiniMapButtonEvent?.Invoke();
        }

        private void OnClickSaveButton()
        {
            ClickSaveButtonEvent?.Invoke();
        }
        
        private void OnChangeSearchTextField(ChangeEvent<string> callback)
        {
            ChangeSearchFieldEvent?.Invoke(callback.newValue);
        }
    }
}  