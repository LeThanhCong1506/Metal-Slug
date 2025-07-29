using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    public class Collection<T> : BaseCollection, IEnumerable<T>,ISerializationCallbackReceiver
    {
        public new T this[int index]
        {
            get
            {
                return Value[index];
            }
            set
            {
                Value[index] = value;
            }
        }

        [SerializeField]
        [ListDrawerSettings(AddCopiesLastElement =  true)]
        [TableList]
        private List<T> _list = new List<T>();
        [System.NonSerialized]
        [ShowInInspector]
        [TableList]
        [HideInEditorMode]
        private List<T> _runtimeList = new List<T>();

        public System.Action onChangeCollection;
        public System.Action<T> onAddElement,onRemoveElement;

        [Button("Raise")]
        public void Raise()
        {
            onChangeCollection?.Invoke();
        }
        public override IList List
        {
            get
            {
                return  Application.isPlaying ? _runtimeList : _list ;
            }
        }

        public List<T> Value
        {
            get { return Application.isPlaying ? _runtimeList : _list; }
            set
            {
                _runtimeList = value;
            }
        }

        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        public void Add(T obj)
        {
            if (!List.Contains(obj))
            {
                List.Add(obj);
                onChangeCollection?.Invoke();
                onAddElement?.Invoke(obj);
            }
           
        }
        public void Remove(T obj)
        {
            if (!List.Contains(obj)) return;
            List.Remove(obj);
            onChangeCollection?.Invoke();
            onRemoveElement?.Invoke(obj);

        }
        public void Clear()
        {
            List.Clear();
            onChangeCollection?.Invoke();
        }
        public bool Contains(T value)
        {
            return List.Contains(value);
        }
        public int IndexOf(T value)
        {
            return List.IndexOf(value);
        }
        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }
        public void Insert(int index, T value)
        {
            List.Insert(index, value);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Value.GetEnumerator();
        }
        public override string ToString()
        {
            return "Collection<" + typeof(T) + ">(" + Count + ")";
        }
        public T[] ToArray() {
            return Value.ToArray();
        }

        public void OnBeforeSerialize()
        {
           
        }

        public virtual void OnAfterDeserialize()
        {
                _runtimeList = new List<T>(_list);

        }
    } 
}
