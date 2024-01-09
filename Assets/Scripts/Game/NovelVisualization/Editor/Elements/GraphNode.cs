using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.NovelVisualization.Editor
{
    public class GraphNode : Node
    {
        public string Key { get; private set; }
        public List<TransitionData> Transitions { get; set; }
        public string MetaData { get; set; }

        public event Action<GraphNode, string> ChangeKeyEvent;
        public event Action<List<GraphElement>> DeleteElementsRequestEvent;

        public void Initialize(Vector2 position)
        {
            Key = "DefaultKey";
            Transitions = new List<TransitionData>();
            MetaData = "Enter Meta Data";
            
            SetPosition(new Rect(position, Vector2.zero));
            
            CreateTransitionPort();
            
            mainContainer.AddToClassList("ng-node__main-container");
            extensionContainer.AddToClassList("ng-node__extension-container");
        }
        
        public void Draw()
        {
            var keyTextField = new TextField()
            {
                value = Key,
            };
            
            keyTextField.RegisterValueChangedCallback(OnKeyFieldChange);

            keyTextField.AddToClassList("ng-node__text-field");
            keyTextField.AddToClassList("ng-node__filename-text-field");
            keyTextField.AddToClassList("ng-node__text-field__hidden");
            
            titleContainer.Insert(0, keyTextField);

            var addTransitionButton = new Button(OnClickAddTransitionButton)
            {
                text = "Add Transition"
            };
            
            addTransitionButton.AddToClassList("ng-node__button");
            
            mainContainer.Insert(1, addTransitionButton);

            var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Input";
            inputContainer.Add(inputPort);

            var foldout = new Foldout()
            {
                text = "MetaData"
            };

            var metaDataTextField = new TextField()
            {
                value = MetaData,
            };
            
            metaDataTextField.AddToClassList("ng-node__text-field");
            metaDataTextField.AddToClassList("ng-node__quote-text-field");
            
            foldout.Add(metaDataTextField);

            extensionContainer.Add(foldout);
            
            RefreshExpandedState();
        }

        private void OnClickAddTransitionButton() => CreateTransitionPort();

        private void CreateTransitionPort()
        {
            var transitionData = new TransitionData()
            {
                key = "Transition",
            };
            
            var transitionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            transitionPort.portName = string.Empty;

            transitionPort.userData = transitionData;

            var transitionTextField = new TextField()
            {
                value = transitionData.key,
            };

            transitionTextField.RegisterValueChangedCallback(callback =>
            {
                transitionData.key = callback.newValue;
            });
                
            transitionTextField.AddToClassList("ng-node__text-field");
            transitionTextField.AddToClassList("ng-node__transition-text-field");
            transitionTextField.AddToClassList("ng-node__text-field__hidden");
            
            var deleteTransitionButton = new Button(() => OnClickDeleteTransitionButton(transitionPort, transitionData))
            {
                text = "X",
            };
                
            deleteTransitionButton.AddToClassList("ng-node__button");

            transitionPort.Add(transitionTextField);
            transitionPort.Add(deleteTransitionButton);
                
            outputContainer.Add(transitionPort);
            
            Transitions.Add(transitionData);
        }

        public void SetBackgroundColor(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        private void OnClickDeleteTransitionButton(Port port, TransitionData transitionData)
        {
            if (Transitions.Count == 1)
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
            
            Transitions.Remove(transitionData);
        }

        private void OnKeyFieldChange(ChangeEvent<string> value)
        {
            var field = (TextField)value.target;

            var lastKey = Key;
            
            Key = value.newValue;
            
            ChangeKeyEvent?.Invoke(this, lastKey);
        }
    }
}