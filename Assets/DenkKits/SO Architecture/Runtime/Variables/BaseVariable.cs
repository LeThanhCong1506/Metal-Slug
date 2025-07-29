using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
    public abstract class BaseVariable : GameEventBase
    {
        
#if UNITY_EDITOR
        public virtual string[] propNames { get; }
#endif
        public abstract bool IsClamped { get; }
        public abstract bool Clampable { get; }
        public abstract bool ReadOnly { get; }
        public abstract System.Type Type { get; }
        public abstract object BaseValue { get; set; }
    }
    public abstract class BaseVariable<T> : BaseVariable,ISerializationCallbackReceiver
    {
        #if UNITY_EDITOR
        [ShowInInspector]
        [HideInEditorMode]
        #endif
        protected T runtimeValue { get; set; }

        private bool IsRunTime => Application.isPlaying; 
        public virtual T ValueRaw
        {
            get => IsRunTime ? runtimeValue : _value;
            set
            {
                if (IsRunTime)
                {
                    runtimeValue = SetValue(value);
                }
                else
                {
                    _value = SetValue(value);
                }
            
            }
        }

        public T PreviousValue { get; set; }
        public virtual T Value
        {
            get => IsRunTime ? runtimeValue : _value;
            set
            {
                PreviousValue = Value;
                if (IsRunTime)
                {
                    runtimeValue = SetValue(value);
                }
                else
                {
                    _value = SetValue(value);
                }
            
                Raise();
            }
        }
        public virtual T MinClampValue
        {
            get
            {
                if(Clampable)
                {
                    return _minClampedValue;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public virtual T MaxClampValue
        {
            get
            {
                if(Clampable)
                {
                    return _maxClampedValue;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public override bool Clampable { get { return false; } }
        public override bool ReadOnly { get { return _readOnly; } }
        public override bool IsClamped { get { return _isClamped; } }
        public override System.Type Type { get { return typeof(T); } }
        public override object BaseValue
        {
            get
            {
                return _value;
            }
            set
            {
                _value = SetValue((T)value);
                Raise();
            }
        }

        [SerializeField]    
        [HideInPlayMode]
        protected T _value = default(T);
        [SerializeField]
        [HideInInspector]
        private bool _readOnly = false;
        [SerializeField]
        [HideInInspector]
        private bool _raiseWarning = true;
        [SerializeField]
        [HideInInspector]
        protected bool _isClamped = false;
        [SerializeField]
        [HideInInspector]
        protected T _minClampedValue = default(T);
        [SerializeField]
        [HideInInspector]
        protected T _maxClampedValue = default(T);

        public virtual T SetValue(BaseVariable<T> value)
        {
            return SetValue(value.Value);
        }
        public virtual T SetValue(T value)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return Value;
            }
            else if(Clampable && IsClamped)
            {
                return ClampValue(value);
            }

            return value;
        }        
        protected virtual T ClampValue(T value)
        {
            return value;
        }
        private void RaiseReadonlyWarning()
        {
            if (!_readOnly || !_raiseWarning)
                return;

            Debug.LogWarning("Tried to set value on " + name + ", but value is readonly!", this);
        }
        public override string ToString()
        {
            return Value == null ? "null" : Value.ToString();
        }
        public static implicit operator T(BaseVariable<T> variable)
        {
            return variable.Value;
        }
 

        public virtual void OnBeforeSerialize()
        {
        
        }

        public bool serializeByJson;
        public virtual void OnAfterDeserialize()
        {
            
            if (serializeByJson)
            {
                runtimeValue = (T)JsonUtility.FromJson(JsonUtility.ToJson(_value), typeof(T));
            }
            else
            {
                runtimeValue = (T) SerializationUtility.CreateCopy( _value);
            }
              
            
        }
    }
    public abstract class BaseVariable<T, TEvent> : BaseVariable<T> where TEvent : UnityEvent<T>
    {
        [SerializeField]
        private TEvent _event = default;

        public override T SetValue(T value)
        {
            T oldValue = Value;
            T newValue = base.SetValue(value);

            if ((newValue == null && oldValue != null) || (newValue != null && !newValue.Equals(oldValue)))
                _event.Invoke(newValue);

            return newValue;
        }
        public void AddListener(UnityAction<T> callback)
        {
            _event.AddListener(callback);
        }
        public void RemoveListener(UnityAction<T> callback)
        {
            _event.RemoveListener(callback);
        }
        public override void RemoveAll()
        {
            base.RemoveAll();
            _event.RemoveAllListeners();
        }
    }
}