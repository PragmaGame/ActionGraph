using UnityEditor;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    public class ActionGraphEditorWindow : EditorWindow
    {
        private ActionGraphView _actionGraphView;
        private ActionGraphToolBarView _actionGraphToolBarView;

        [MenuItem("Graph/Action Graph")]
        public static void Open()
        {
            GetWindow<ActionGraphEditorWindow>("Action Graph");
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
            _actionGraphView = new ActionGraphView();

            _actionGraphView.StretchToParentSize();
            
            rootVisualElement.Add(_actionGraphView);
        }

        private void AddToolBar()
        {
            _actionGraphToolBarView = new ActionGraphToolBarView();
            
            var toolbar = _actionGraphToolBarView.Initialize();

            _actionGraphToolBarView.ClickSaveButtonEvent += OnClickSaveButton;
            _actionGraphToolBarView.ClickLoadButtonEvent += OnClickLoadButton;
            _actionGraphToolBarView.ClickMiniMapButtonEvent += OnClickMiniMapButton;
            _actionGraphToolBarView.ChangeSearchFieldEvent += OnChangeSearchField;

            rootVisualElement.Add(toolbar);
        }

        private void OnClickSaveButton(string filePath)
        {
            ActionGraphSaveUtility.Save(_actionGraphView, filePath);
        }

        private void OnClickLoadButton(string filePath)
        {
            ActionGraphSaveUtility.Load(_actionGraphView, filePath);
        }
        
        private void OnClickMiniMapButton()
        {
            _actionGraphView.SwitchActiveMiniMap();
        }

        private void OnChangeSearchField(string value)
        {
            _actionGraphView.LookAt(value);
        }
    }
}