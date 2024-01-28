using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    public class ActionGraphToolBarView
    {
        private ActionGraphToolBarViewConfig _config;

        private TextField _fileNameTextField;
        
        private Button _saveButton;
        private Button _loadButton;
        private Button _miniMapButton;

        public event Action<string> ClickSaveButtonEvent;
        public event Action<string> ClickLoadButtonEvent;
        public event Action<string> ChangeSearchFieldEvent;
        public event Action ClickMiniMapButtonEvent;
        
        public Toolbar Toolbar { get; private set; }

        public ActionGraphToolBarView(ActionGraphToolBarViewConfig config)
        {
            _config = config;
            
            Toolbar = new Toolbar();

            _fileNameTextField = new TextField()
            {
                label = "Search Node:",
            };

            _fileNameTextField.RegisterValueChangedCallback(OnChangeFileNameTextField);
            
            Toolbar.Add(_fileNameTextField);

            _saveButton = new Button(OnClickSaveButton)
            {
                text = "Save",
            };
            
            Toolbar.Add(_saveButton);
            
            _loadButton = new Button(OnClickLoadButton)
            {
                text = "Load",
            };
            
            Toolbar.Add(_loadButton);
            
            _miniMapButton = new Button(OnClickMiniMapButton)
            {
                text = "Mini Map",
            };
            
            Toolbar.Add(_miniMapButton);
            
            Toolbar.styleSheets.Add(_config.StyleSheet);
        }

        private void OnClickLoadButton()
        {
            var filePath = EditorUtility.OpenFilePanel("Action Graphs", "Assets/", "asset");
            
            ClickLoadButtonEvent?.Invoke(filePath);
        }

        private void OnClickSaveButton()
        {
            var filePath = EditorUtility.SaveFilePanel("Action Graphs", "Assets/","ActionGraphFileName","asset");
            
            ClickSaveButtonEvent?.Invoke(filePath);
        }

        private void OnClickMiniMapButton()
        {
            ClickMiniMapButtonEvent?.Invoke();
        }

        private void OnChangeFileNameTextField(ChangeEvent<string> callback)
        {
            ChangeSearchFieldEvent?.Invoke(callback.newValue);
        }
    }
}  