using System;

namespace Pragma.ActionGraph.Runtime.Common
{
    [Serializable]
    public class SerializedTuple<TValue1, TValue2>
    {
        public TValue1 value1;
        public TValue2 value2;

        public SerializedTuple()
        {
            
        }
        
        public SerializedTuple(TValue1 value1, TValue2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }
        
        public SerializedTuple(SerializedTuple<TValue1, TValue2> tuple)
        {
            this.value1 = tuple.value1;
            this.value2 = tuple.value2;
        }
    }
}