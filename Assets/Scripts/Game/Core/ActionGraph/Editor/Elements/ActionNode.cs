﻿using System;
using System.Collections.Generic;
using Game.Core.ActionGraph.Runtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Game.Core.ActionGraph.Editor
{
    public class ActionNode : Node
    {
        public ActionNodeData Data { get; private set; }

        public event Action<List<GraphElement>> DeleteElementsRequestEvent;
        public event Func<ActionNode, string, string, string> ChangeKeyFunc; 

        private Port _inputPort;

        public string Key => Data.Key;

        public ActionNode(ActionNodeData data)
        {
            Data = data;

            mainContainer.AddToClassList(StylesConstant.NodeConstant.MAIN_CONTAINER);
            extensionContainer.AddToClassList(StylesConstant.NodeConstant.EXTENSION_CONTAINER);

            CreateElements();
        }

        public override void OnSelected()
        {
            base.OnSelected();
            
            Selection.SetActiveObjectWithContext(Data, Data);
        }

        public void AddKeyPostfix(string value)
        {
            Data.Key += value;
            Data.name = Data.Key;
        }

        public override void UpdatePresenterPosition()
        {
            Data.Position = GetPosition().position;
        }

        private void CreateElements()
        {
            var keyTextField = new TextField()
            {
                value = Key,
            };
            
            keyTextField.RegisterValueChangedCallback(OnKeyFieldChange);

            keyTextField.AddToClassList(StylesConstant.NodeConstant.NODE_TEXT_FIELD);
            keyTextField.AddToClassList(StylesConstant.NodeConstant.NODE_FILENAME_TEXT_FIELD);
            keyTextField.AddToClassList(StylesConstant.NodeConstant.NODE_TEXT_FIELD_HIDDEN);

            titleContainer.Clear();
            titleContainer.Add(keyTextField);

            var addTransitionButton = new Button(OnClickAddTransitionButton)
            {
                text = "Add Transition"
            };
            
            addTransitionButton.AddToClassList(StylesConstant.NodeConstant.NODE_BUTTON);
            
            mainContainer.Insert(1, addTransitionButton);

            _inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            _inputPort.portName = "Input";
            inputContainer.Add(_inputPort);
            
            CreateTransitions();

            RefreshExpandedState();
        }

        private void OnClickAddTransitionButton() => CreateTransitionPort();

        private void CreateTransitions()
        {
            if (Data.Transitions == null || Data.Transitions.Count < 1)
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
            if (Data.Transitions.Count == 1)
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

        private void OnKeyFieldChange(ChangeEvent<string> value)
        {
            Data.Key = ChangeKeyFunc.Invoke(this, value.previousValue, value.newValue);
            Data.name = Data.Key;

            (value.target as TextField)?.SetValueWithoutNotify(Key);

            foreach (var edge in _inputPort.connections)
            {
                var transitionData = (TransitionData)edge.output.userData;
                transitionData.nodeKey = Key;
            }
        }
    }
}