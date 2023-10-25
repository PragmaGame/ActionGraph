using Game.NovelVisualization.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.NovelVisualization.Editor
{
    public class MultipleNode : CustomNode
    {
        public override void Initialize(string key, NovelGraphView novelGraphView, Vector2 position)
        {
            base.Initialize(key, novelGraphView, position);

            TransitionType = TransitionType.Multiple;

            TransitionSaveData choiceData = new TransitionSaveData()
            {
                Text = "New Transition"
            };

            Transitions.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTAINER */

            Button addTransitionButton = GraphElementUtility.CreateButton("Add Transition", () =>
            {
                TransitionSaveData choiceData = new TransitionSaveData()
                {
                    Text = "New Transition"
                };

                Transitions.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });

            addTransitionButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addTransitionButton);

            /* OUTPUT CONTAINER */

            foreach (TransitionSaveData choice in Transitions)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        private Port CreateChoicePort(object userData)
        {
            var transitionPort = this.CreatePort();

            transitionPort.userData = userData;

            TransitionSaveData transitionData = (TransitionSaveData) userData;

            Button deleteTransitionButton = GraphElementUtility.CreateButton("X", () =>
            {
                if (Transitions.Count == 1)
                {
                    return;
                }

                if (transitionPort.connected)
                {
                    graphView.DeleteElements(transitionPort.connections);
                }

                Transitions.Remove(transitionData);

                graphView.RemoveElement(transitionPort);
            });

            deleteTransitionButton.AddToClassList("ds-node__button");

            TextField transitionTextField = GraphElementUtility.CreateTextField(transitionData.Text, null, callback =>
            {
                transitionData.Text = callback.newValue;
            });

            transitionTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__choice-text-field"
            );

            transitionPort.Add(transitionTextField);
            transitionPort.Add(deleteTransitionButton);

            return transitionPort;
        }
    }
}