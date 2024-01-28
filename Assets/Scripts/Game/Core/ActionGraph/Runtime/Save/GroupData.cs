using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class GroupData
    {
        public string key;
        public List<string> ownedNodesKeys;

#if UNITY_EDITOR
        [Space(25)]
        [InfoBox("Editor Only")]
        public Vector2 position;
#endif
        
        public GroupData()
        {
        }
        
        protected GroupData(GroupData data)
        {
            key = data.key;
            ownedNodesKeys = data.ownedNodesKeys.ToList();

#if UNITY_EDITOR
            position = data.position;
#endif
        }

        public GroupData Clone() => new(this);
    }
}