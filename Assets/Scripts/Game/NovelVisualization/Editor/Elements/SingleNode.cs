using Game.NovelVisualization.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    public class CustomSingleChoiceNode : CustomNode
    {
        public override void Initialize(string nodeName, NovelGraphView novelGraphView, Vector2 position)
        {
            base.Initialize(nodeName, novelGraphView, position);

            DialogueType = TransitionType.Single;

            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "Next Dialogue"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            /* OUTPUT CONTAINER */

            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
