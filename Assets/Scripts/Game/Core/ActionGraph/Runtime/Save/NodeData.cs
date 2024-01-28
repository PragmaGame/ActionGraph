using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class NodeData
    {
        public string key;
        public string metaData;
        public List<TransitionData> transitions;

#if UNITY_EDITOR
        [Space(25)]
        [InfoBox("Editor Only")]
        public Vector2 position;
#endif
        
        public NodeData()
        {
        }
        
        protected NodeData(NodeData data)
        {
            key = data.key;
            metaData = data.metaData;
            transitions = data.transitions.Select(transition => transition.Clone()).ToList();

#if UNITY_EDITOR
            position = data.position;
#endif
        }

        public NodeData Clone() => new(this);
    }
}