using Game.Core.ActionGraph.Runtime;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    public class ActionGraphEditorWindow : EditorWindow
    {
        private ActionGraphEditorConfig _config;
        
        private ActionGraphView _actionGraphView;
        private ActionGraphToolBarView _actionGraphToolBarView;
        
        [OnOpenAsset(1)]
        public static bool Open(int instanceID)
        {
            UnityEngine.Object asset = EditorUtility.InstanceIDToObject(instanceID);

            if (asset is ActionGraphData data)
            {
                var window = GetWindow<ActionGraphEditorWindow>("Action Graph");
                window.Load(data);
            }

            return false;
        }

        public void Load(ActionGraphData data)
        {
            _actionGraphView.LoadData(data);
        }

        private void OnEnable()
        {
            _config = (ActionGraphEditorConfig)EditorGUIUtility.Load("NovelGraph/ActionGraphEditorConfig.asset");
            
            AddGraphView();
            AddToolBar();
            AddStyles();
        }

        private void AddStyles()
        {
            rootVisualElement.styleSheets.Add(_config.Variables);
        }

        private void AddGraphView()
        {
            _actionGraphView = new ActionGraphView(_config.ActionGraphViewConfig);

            _actionGraphView.StretchToParentSize();
            
            rootVisualElement.Add(_actionGraphView);
        }

        private void AddToolBar()
        {
            _actionGraphToolBarView = new ActionGraphToolBarView(_config.ActionGraphToolBarViewConfig);

            _actionGraphToolBarView.ClickSaveButtonEvent += OnClickSaveButton;
            _actionGraphToolBarView.ClickLoadButtonEvent += OnClickLoadButton;
            _actionGraphToolBarView.ClickMiniMapButtonEvent += OnClickMiniMapButton;
            _actionGraphToolBarView.ChangeSearchFieldEvent += OnChangeSearchField;

            rootVisualElement.Add(_actionGraphToolBarView.Toolbar);
        }

        private void OnClickSaveButton(string filePath)
        {
            //ActionGraphSaveUtility.Save(_actionGraphView, filePath);
        }

        private void OnClickLoadButton(string filePath)
        {
            //ActionGraphSaveUtility.Load(_actionGraphView, filePath);
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