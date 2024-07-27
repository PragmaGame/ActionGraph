using System;
using System.Collections.Generic;
using ActionGraph.Editor.Windows;
using ActionGraph.Runtime.Save;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace ActionGraph.Editor.Elements
{
    public class ActionNode : Node
    {
        public ActionNodeData Data { get; private set; }

        public event Action<List<GraphElement>> DeleteElementsRequestEvent;

        private Port _inputPort;

        public string Key => Data.Key;
        public string Tag => Data.Tag;

        private TextField _debugText;

        public ActionNode(ActionNodeData data)
        {
            Data = data;

            title = "Action";
            
            mainContainer.AddToClassList(StylesConstant.NodeConstant.MAIN_CONTAINER);
            extensionContainer.AddToClassList(StylesConstant.NodeConstant.EXTENSION_CONTAINER);

            CreateElements();
        }

        public override void OnSelected()
        {
            base.OnSelected();
            
            Selection.SetActiveObjectWithContext(Data, Data);
        }

        public override void UpdatePresenterPosition()
        {
            Data.Position = GetPosition().position;
        }

        private void CreateElements()
        {
            var tagTextField = new TextField()
            {
                value = Data.Tag,
                isDelayed = true
            };
            
            tagTextField.RegisterValueChangedCallback(OnTagFieldChange);

            tagTextField.AddToClassList(StylesConstant.NodeConstant.NODE_TEXT_FIELD);
            tagTextField.AddToClassList(StylesConstant.NodeConstant.NODE_FILENAME_TEXT_FIELD);
            //keyTextField.AddToClassList(StylesConstant.NodeConstant.NODE_TEXT_FIELD_HIDDEN);

            //titleContainer.Clear();
            titleButtonContainer.Clear();
            mainContainer.Insert(1, tagTextField);

            var addTransitionButton = new Button(OnClickAddTransitionButton)
            {
                text = "Add Transition"
            };
            
            addTransitionButton.AddToClassList(StylesConstant.NodeConstant.NODE_BUTTON);
            
            mainContainer.Insert(2, addTransitionButton);

            _inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            _inputPort.portName = "Input";
            inputContainer.Add(_inputPort);

            _debugText = new TextField()
            {
                multiline = true,
                focusable = false,
                tooltip = "Debug",
            };
            
            _debugText.AddToClassList(StylesConstant.NodeConstant.QUOTE_TEXT_FIELD);
            
            RefreshDebugInfo();

            extensionContainer.Add(_debugText);

            CreateTransitions();

            RefreshExpandedState();
        }

        private void RefreshDebugInfo()
        {
            _debugText.value = Data.Command.GetInfo();
        }
        
        private void OnClickAddTransitionButton() => CreateTransitionPort();

        private void CreateTransitions()
        {
            if (Data.Transitions.Count < 1)
            {
                CreateTransitionPort();
            }
            else
            {
                foreach (var transitionData in Data.Transitions)
                {
                    CreateTransitionPort(transitionData);
                }
            }
        }
        
        private void CreateTransitionPort(TransitionData transition = null)
        {
            if (transition == null)
            {
                transition = new TransitionData();
                Data.Transitions.Add(transition);
            }

            var transitionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            transitionPort.portName = string.Empty;

            transitionPort.userData = transition;

            var deleteTransitionButton = new Button(() => OnClickDeleteTransitionButton(transitionPort, transition))
            {
                text = "X",
            };
                
            deleteTransitionButton.AddToClassList(StylesConstant.NodeConstant.NODE_BUTTON);
            
            transitionPort.Add(deleteTransitionButton);

            outputContainer.Add(transitionPort);
        }

        public void SetBackgroundColor(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        private void OnClickDeleteTransitionButton(Port port, TransitionData transitionData)
        {
            if (Data.Transitions.Count <= 1)
            {
                return;
            }
            
            var elements = new List<GraphElement>();
            
            if (port.connected)
            {
                elements.AddRange(port.connections);
            }

            elements.Add(port);
            
            DeleteElementsRequestEvent?.Invoke(elements);
            
            Data.Transitions.Remove(transitionData);
        }

        private void OnTagFieldChange(ChangeEvent<string> value)
        {
            Data.Tag = value.newValue;
            Data.name = Data.Tag;

            (value.target as TextField)?.SetValueWithoutNotify(Data.Tag);
        }
    }
}