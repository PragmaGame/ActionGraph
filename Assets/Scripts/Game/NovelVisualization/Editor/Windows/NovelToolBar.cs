using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.NovelVisualization.Editor
{
    public class NovelToolBar
    {
        private Toolbar _toolbar;

        private TextField _fileNameTextField;
        
        private Button _saveButton;
        private Button _loadButton;
        private Button _miniMapButton;

        public event Action<string> ClickSaveButtonEvent;
        public event Action<string> ClickLoadButtonEvent;
        public event Action<string> ChangeSearchFieldEvent;
        public event Action ClickMiniMapButtonEvent;

        public Toolbar Initialize()
        {
            _toolbar = new Toolbar();

            _fileNameTextField = new TextField()
            {
                label = "Search Node:",
            };

            _fileNameTextField.RegisterValueChangedCallback(OnChangeFileNameTextField);
            
            _toolbar.Add(_fileNameTextField);

            _saveButton = new Button(OnClickSaveButton)
            {
                text = "Save",
            };
            
            _toolbar.Add(_saveButton);
            
            _loadButton = new Button(OnClickLoadButton)
            {
                text = "Load",
            };
            
            _toolbar.Add(_loadButton);
            
            _miniMapButton = new Button(OnClickMiniMapButton)
            {
                text = "Mini Map",
            };
            
            _toolbar.Add(_miniMapButton);

            var styleSheet = (StyleSheet)EditorGUIUtility.Load("NovelGraph/NovelGraphToolbarStyles.uss");
            _toolbar.styleSheets.Add(styleSheet);
            
            return _toolbar;
        }

        private void OnClickLoadButton()
        {
            var filePath = EditorUtility.OpenFilePanel("Novel Graphs", "Assets/", "asset");
            
            ClickLoadButtonEvent?.Invoke(filePath);
        }

        private void OnClickSaveButton()
        {
            var filePath = EditorUtility.SaveFilePanel("Novel Graphs", "Assets/","NovelGraphFileName","asset");
            
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