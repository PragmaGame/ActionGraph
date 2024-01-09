using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.NovelVisualization.Editor
{
    public class NovelEditorWindow : EditorWindow
    {
        [MenuItem("Novel/Novel Graph")]
        public static void Open()
        {
            GetWindow<NovelEditorWindow>("Novel Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }

        private void AddStyles()
        {
            var styleSheet = (StyleSheet)EditorGUIUtility.Load("NovelGraph/NovelGraphVariables.uss");
            
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void AddGraphView()
        {
            var graphView = new NovelGraphView();
            
            graphView.StretchToParentSize();
            
            rootVisualElement.Add(graphView);
        }

        private void AddToolBar()
        {
            var toolbar = new Toolbar();
        }
    }
}