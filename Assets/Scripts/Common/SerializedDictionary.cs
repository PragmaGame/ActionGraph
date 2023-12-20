using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Common
{
    public interface IKeyValuePair<out TKey, out TValue>
    {
        TKey Key { get; }
        TValue Value { get; }
    }

    [Serializable]
    public struct Kvp<TKey, TValue> : IKeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue val;
        public TKey Key { get => key; set => key = value; }
        public TValue Value { get => val; set => val = value; }
    }

    [Serializable]
    public abstract class SerializedDictionary<TKey, TValue> : SerializedDictionary<TKey, TValue, Kvp<TKey, TValue>>
    {
        public override Kvp<TKey, TValue> Create(KeyValuePair<TKey, TValue> kvp) => new Kvp<TKey, TValue>() { Key = kvp.Key, Value = kvp.Value };
    }

    [Serializable]
    public abstract class SerializedDictionary<TKey, TValue, TClass> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver where TClass : IKeyValuePair<TKey, TValue>
    {
        [SerializeField, HideInInspector] private List<TClass> data = new List<TClass>();

        void ISerializationCallbackReceiver.OnAfterDeserialize() => Deserialize();
        void ISerializationCallbackReceiver.OnBeforeSerialize() => Serialize();

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) => Deserialize();

        [OnSerializing]
        internal void OnSerializing(StreamingContext context) => Serialize();

        public SerializedDictionary() { }
        public SerializedDictionary(Dictionary<TKey, TValue> dictionary) : base(dictionary) { }

        private void Deserialize()
        {
            Clear();

            for (var i = 0; i < data.Count; i++)
            {
                var kvp = data[i];
                this[kvp.Key] = kvp.Value;
            }
        }

        private void Serialize()
        {
            data.Clear();

            foreach (var item in this)
            {
                data.Add(Create(item));
            }
        }

        public void Set(TKey key, TValue value)
        {
            if (!TryAdd(key, value))
                this[key] = value;
        }

        public TValue Get(TKey key)
        {
            TryGetValue(key, out var value);

            return value;
        }

        public abstract TClass Create(KeyValuePair<TKey, TValue> kvp);
    }
}