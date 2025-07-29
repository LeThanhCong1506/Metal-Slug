using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ScriptableObjectArchitecture
{
    public abstract class GameEventBase<T> : GameEventBase, IGameEvent<T>, IStackTraceObject
    {
        private readonly List<IGameEventListener<T>> _typedListeners = new List<IGameEventListener<T>>();
        private readonly List<System.Action<T>> _typedActions = new List<System.Action<T>>();
    //    public bool RaiseGlobalEvent;
        [SerializeField]
        protected T _debugValue = default(T);

        protected T _runTimeValue = default(T);
        public T RaiseValue
        {
            set
            {
                _runTimeValue = value;
                Raise(_runTimeValue); 
            }
            get => _runTimeValue;
        }

        /*public override void Raise()
        {
            base.Raise();
            RaiseDebugValue();
        }*/

        public void RaiseDebugValue()
        {
            Raise(_debugValue);
        }
        public void Raise(T value)
        {
            // if (RaiseGlobalEvent)
            // {
            //     MMEventManager.TriggerEvent(value);
            // }
            AddStackTrace(value);

            for (int i = _typedListeners.Count - 1; i >= 0; i--)
                _typedListeners[i].OnEventRaised(value);

            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();

            for (int i = _typedActions.Count - 1; i >= 0; i--)
                _typedActions[i](value);

            for (int i = _actions.Count - 1; i >= 0; i--)
                _actions[i]();
        }
        public void AddListener(IGameEventListener<T> listener)
        {
            if (!_typedListeners.Contains(listener))
                _typedListeners.Add(listener);
        }
        public void RemoveListener(IGameEventListener<T> listener)
        {
            if (_typedListeners.Contains(listener))
                _typedListeners.Remove(listener);
        }
        public void AddListener(System.Action<T> action)
        {
            if (!_typedActions.Contains(action))
                _typedActions.Add(action);
        }
        public void RemoveListener(System.Action<T> action)
        {
            if (_typedActions.Contains(action))
                _typedActions.Remove(action);
        }
        public override string ToString()
        {
            return "GameEventBase<" + typeof(T) + ">";
        }
    }
    public abstract class GameEventBase : SOArchitectureBaseObject, IGameEvent, IStackTraceObject
    {
        protected readonly List<IGameEventListener> _listeners = new List<IGameEventListener>();
        protected readonly List<System.Action> _actions = new List<System.Action>();
    
        [ShowInInspector]
        [HideInEditorMode]
        public List<StackTraceEntry> StackTraces => _stackTraces;
        private List<StackTraceEntry> _stackTraces = new List<StackTraceEntry>();

        public void AddStackTrace()
        {
#if UNITY_EDITOR
            if (SOArchitecturePreferences.IsDebugEnabled)
                _stackTraces.Insert(0, StackTraceEntry.Create());
#endif
        }
        public void AddStackTrace(object value)
        {
#if UNITY_EDITOR
            if(SOArchitecturePreferences.IsDebugEnabled)
                _stackTraces.Insert(0, StackTraceEntry.Create(value));
#endif
        }
        [Button("Raise")]
        public virtual  void Raise()
        {
            AddStackTrace();

            for (int i = _listeners.Count - 1; i >= 0; i--)
                _listeners[i].OnEventRaised();

            for (int i = _actions.Count - 1; i >= 0; i--)
                _actions[i]();
        }
        public void AddListener(IGameEventListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }
        public void RemoveListener(IGameEventListener listener)
        {
            if (_listeners.Contains(listener))
                _listeners.Remove(listener);
        }
        public void AddListener(System.Action action)
        {
            if (!_actions.Contains(action))
                _actions.Add(action);
        }
        public void RemoveListener(System.Action action)
        {
            if (_actions.Contains(action))
                _actions.Remove(action);
        }
        public virtual void RemoveAll()
        {
            _listeners.RemoveRange(0, _listeners.Count);
            _actions.RemoveRange(0, _actions.Count);
        }
    } 
}