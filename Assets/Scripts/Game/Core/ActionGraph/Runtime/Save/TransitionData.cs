using System;

namespace Game.Core.ActionGraph.Runtime
{
    [Serializable]
    public class TransitionData
    {
        public string value;
        public string nodeKey;

        public TransitionData()
        {
        }
        
        public TransitionData(TransitionData data)
        {
            value = data.value;
            nodeKey = data.nodeKey;
        }

        public TransitionData Clone() => new(this);
    }
} 