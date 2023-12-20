using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Common
{
    [Serializable]
    public abstract class SerializedHashSet<T> : ISerializationCallbackReceiver, ISet<T>,
                                                 IReadOnlyCollection<T>
    {
        [SerializeField, HideInInspector] private List<T> values  = new List<T>();
        
        private HashSet<T> _hashSet = new HashSet<T>();

        void ISerializationCallbackReceiver.OnAfterDeserialize() => Deserialize();
        void ISerializationCallbackReceiver.OnBeforeSerialize() => Serialize();
        
        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) => Deserialize();
        [OnSerializing]
        internal void OnSerializing(StreamingContext context) => Serialize();

        protected SerializedHashSet() {}
        
        protected SerializedHashSet(HashSet<T> hashSet)
        {
            _hashSet = hashSet;
        }

        private void Deserialize()
        {
            Clear();
            
            foreach (var val in values)
            {
                Add(val);
            }
        }

        private void Serialize()
        {
            values.Clear();
            
            foreach (var val in this)
            {
                values.Add(val);
            }
        }
        
        
        /// <summary>
        /// Implementations IReadOnlyCollection and ISet
        /// </summary>
        public int Count => _hashSet.Count;
        
        public bool IsReadOnly => false;
        
        bool ISet<T>.Add(T item) => _hashSet.Add(item);
        
        bool ICollection<T>.Remove(T item) => _hashSet.Remove(item);
        
        public void ExceptWith(IEnumerable<T> other) => _hashSet.ExceptWith(other);
        
        public void IntersectWith(IEnumerable<T> other) => _hashSet.IntersectWith(other);
        
        public bool IsProperSubsetOf(IEnumerable<T> other) => _hashSet.IsProperSubsetOf(other);
        
        public bool IsProperSupersetOf(IEnumerable<T> other) => _hashSet.IsProperSupersetOf(other);
        
        public bool IsSubsetOf(IEnumerable<T> other) => _hashSet.IsSubsetOf(other);
        
        public bool IsSupersetOf(IEnumerable<T> other) => _hashSet.IsSupersetOf(other);
        
        public bool Overlaps(IEnumerable<T> other) => _hashSet.Overlaps(other);
        
        public bool SetEquals(IEnumerable<T> other) => _hashSet.SetEquals(other);
        
        public void SymmetricExceptWith(IEnumerable<T> other) => _hashSet.SymmetricExceptWith(other);
        
        public void UnionWith(IEnumerable<T> other) => _hashSet.UnionWith(other);
        
        public void Add(T item) => _hashSet.Add(item);
        
        public void Clear() => _hashSet.Clear();
        
        public bool Contains(T item) => _hashSet.Contains(item);
        
        public void CopyTo(T[] array, int arrayIndex) => _hashSet.CopyTo(array, arrayIndex);
        
        public IEnumerator<T> GetEnumerator() => _hashSet.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}