using Game.NovelVisualization.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.NovelVisualization.Editor
{
    public class SingleNode : CustomNode
    {
        public override void Initialize(string key, NovelGraphView novelGraphView, Vector2 position)
        {
            base.Initialize(key, novelGraphView, position);

            TransitionType = TransitionType.Single;

            TransitionSaveData transitionData = new TransitionSaveData()
            {
                Text = "Next Transition"
            };

            Transitions.Add(transitionData);
        }

        public override void Draw()
        {
            base.Draw();

            /* OUTPUT CONTAINER */

            foreach (TransitionSaveData choice in Transitions)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
