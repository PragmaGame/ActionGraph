using System.Collections.Generic;
using Game.NovelVisualization.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    public class NovelSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private NovelGraphView graphView;
        private Texture2D indentationIcon;

        public void Initialize(NovelGraphView novelGraphView)
        {
            graphView = novelGraphView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Elements")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Nodes"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", indentationIcon))
                {
                    userData = TransitionType.Single,
                    level = 2
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", indentationIcon))
                {
                    userData = TransitionType.Multiple,
                    level = 2
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Groups"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentationIcon))
                {
                    userData = new Group(),
                    level = 2
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (SearchTreeEntry.userData)
            {
                case TransitionType.Single:
                {
                    SingleNode singleNode = (SingleNode) graphView.CreateNode("DialogueName", TransitionType.Single, localMousePosition);

                    graphView.AddElement(singleNode);

                    return true;
                }

                case TransitionType.Multiple:
                {
                    MultipleNode multipleNode = (MultipleNode) graphView.CreateNode("DialogueName", TransitionType.Multiple, localMousePosition);

                    graphView.AddElement(multipleNode);

                    return true;
                }

                case Group _:
                {
                    graphView.CreateGroup("DialogueGroup", localMousePosition);

                    return true;
                }

                default:
                {
                    return false;
                }
            }
        }
    }
}