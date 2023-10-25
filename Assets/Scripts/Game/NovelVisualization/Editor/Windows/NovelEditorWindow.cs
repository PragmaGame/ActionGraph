using System.IO;
using Game.NovelVisualization.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.NovelVisualization.Editor
{
    public class NovelEditorWindow : EditorWindow
    {
        private NovelGraphView graphView;

        private readonly string defaultFileName = "NovelGraphFileName";

        private static TextField fileNameTextField;
        private Button saveButton;
        private Button miniMapButton;

        [MenuItem("Novel/Novel Graph")]
        public static void Open()
        {
            GetWindow<NovelEditorWindow>("Novel Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();

            AddStyles();
        }

        private void AddGraphView()
        {
            graphView = new NovelGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = GraphElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveButton = GraphElementUtility.CreateButton("Save", () => Save());

            Button loadButton = GraphElementUtility.CreateButton("Load", () => Load());
            Button clearButton = GraphElementUtility.CreateButton("Clear", () => Clear());
            Button resetButton = GraphElementUtility.CreateButton("Reset", () => ResetGraph());

            miniMapButton = GraphElementUtility.CreateButton("Minimap", () => ToggleMiniMap());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(miniMapButton);

            toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Roger!");

                return;
            }

            GraphIOUtility.Initialize(graphView, fileNameTextField.value);
            GraphIOUtility.Save();
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            GraphIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            GraphIOUtility.Load();
        }

        private void Clear()
        {
            graphView.ClearGraph();
        }

        private void ResetGraph()
        {
            Clear();

            UpdateFileName(defaultFileName);
        }

        private void ToggleMiniMap()
        {
            graphView.ToggleMiniMap();

            miniMapButton.ToggleInClassList("ds-toolbar__button__selected");
        }

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }
    }
}