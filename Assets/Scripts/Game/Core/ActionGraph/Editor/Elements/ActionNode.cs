using System;
using System.Collections.Generic;
using Game.Core.ActionGraph.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Core.ActionGraph.Editor
{
    public class ActionNode : Node
    {
        public string Key { get; private set; }
        public List<TransitionData> Transitions { get; set; }
        public string MetaData { get; set; }
        
        public event Action<List<GraphElement>> DeleteElementsRequestEvent;
        public event Func<ActionNode, string, string, string> ChangeKeyFunc; 

        private Port _inputPort;
        
        public void Initialize(string key, List<TransitionData> transitionsDates = null, string metaData = null)
        {
            Key = key;
            Transitions = new List<TransitionData>();

            if (transitionsDates == null)
            {
                CreateTransitionPort();
            }
            else
            {
                foreach (var transitionData in transitionsDates)
                {
                    CreateTransitionPort(transitionData);
                }
            }
            
            MetaData = metaData ?? "Enter Meta Data";

            mainContainer.AddToClassList("ng-node__main-container");
            extensionContainer.AddToClassList("ng-node__extension-container");

            CreateElements();
        }
        
        private void CreateElements()
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

            _inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            _inputPort.portName = "Input";
            inputContainer.Add(_inputPort);

            var foldout = new Foldout()
            {
                text = "MetaData"
            };

            var metaDataTextField = new TextField()
            {
                value = MetaData,
                multiline = true,
            };
            
            metaDataTextField.AddToClassList("ng-node__text-field");
            metaDataTextField.AddToClassList("ng-node__quote-text-field");

            
            // var a = Enum.GetNames(typeof(MetaDataTypes)).ToList();
            // var metaDataDropdown = new DropdownField(a, 0);

            foldout.Add(metaDataTextField);
            //foldout.Add(metaDataDropdown);

            extensionContainer.Add(foldout);
            
            RefreshExpandedState();
        }

        private void OnClickAddTransitionButton() => CreateTransitionPort();

        private void CreateTransitionPort(TransitionData transitionData = null)
        {
            transitionData ??= new TransitionData()
            {
                value = "transition",
            };
            
            var transitionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            transitionPort.portName = string.Empty;

            transitionPort.userData = transitionData;

            var transitionTextField = new TextField()
            {
                value = transitionData.value,
            };

            transitionTextField.RegisterValueChangedCallback(callback =>
            {
                transitionData.value = callback.newValue;
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
            Key = ChangeKeyFunc?.Invoke(this, value.previousValue, value.newValue);
            
            (value.target as TextField)?.SetValueWithoutNotify(Key);

            foreach (var edge in _inputPort.connections)
            {
                var transitionData = (TransitionData)edge.output.userData;
                transitionData.nodeKey = Key;
            }
        }
    }
}