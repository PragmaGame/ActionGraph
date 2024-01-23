using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.NovelVisualization.Editor
{
    public class NovelEditorWindow : EditorWindow
    {
        private NovelGraphView _novelGraphView;
        private NovelToolBar _novelToolBar;

        [MenuItem("Novel/Novel Graph")]
        public static void Open()
        {
            GetWindow<NovelEditorWindow>("Novel Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolBar();
            
            AddStyles();
        }

        private void AddStyles()
        {
            var styleSheet = (StyleSheet)EditorGUIUtility.Load("NovelGraph/NovelGraphVariables.uss");
            
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void AddGraphView()
        {
            _novelGraphView = new NovelGraphView();

            _novelGraphView.StretchToParentSize();
            
            rootVisualElement.Add(_novelGraphView);
        }

        private void AddToolBar()
        {
            _novelToolBar = new NovelToolBar();
            
            var toolbar = _novelToolBar.Initialize();

            _novelToolBar.ClickSaveButtonEvent += OnClickSaveButton;
            _novelToolBar.ClickLoadButtonEvent += OnClickLoadButton;
            _novelToolBar.ClickMiniMapButtonEvent += OnClickMiniMapButton;
            _novelToolBar.ChangeSearchFieldEvent += OnChangeSearchField;

            rootVisualElement.Add(toolbar);
        }

        private void OnClickSaveButton(string filePath)
        {
            GraphSaveUtility.Save(_novelGraphView, filePath);
        }

        private void OnClickLoadButton(string filePath)
        {
            GraphSaveUtility.Load(_novelGraphView, filePath);
        }
        
        private void OnClickMiniMapButton()
        {
            _novelGraphView.SwitchActiveMiniMap();
        }

        private void OnChangeSearchField(string value)
        {
            _novelGraphView.LookAt(value);
        }
    }
}