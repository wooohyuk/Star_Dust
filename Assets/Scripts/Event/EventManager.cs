using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Event
{
    public class EventListener
    {
        public List<object> listeners = new List<object>();
        public void Invoke()
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                ((System.Action)listeners[i])();
            }
        }

        public void Invoke<T>(T arg)
        {
            for (int i = 0; i < listeners.Count; i++)
            {
                ((System.Action<T>)listeners[i])(arg);
            }
        }

        public void Clear()
        {
            listeners.Clear();
        }
    }

    public class EventManager : MonoBehaviour
    {
        private static EventManager _sInstance;

        public static EventManager Instance
        {
            get
            {
                if (_sInstance == null)
                {
                    GameObject newGameObject = new GameObject("_EventManager");
                    _sInstance = newGameObject.AddComponent<EventManager>();
                }

                return _sInstance;
            }
        }

        private Dictionary<string, EventListener> events = new Dictionary<string, EventListener>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary> 이벤트 등록 </summary>
        public void AddEvent(string key, System.Action call)
        {
            EventListener listener;
            if (events.TryGetValue(key, out listener))
            {
                if (!listener.listeners.Contains(call))
                {
                    listener.listeners.Add(call);
                }
            }
            else
            {
                listener = new EventListener();
                listener.listeners.Add(call);
                events.Add(key, listener);
            }
        }

        /// <summary> 파라미터 없는 함수의 호출 </summary>
        public void InvokeEvent(string key)
        {
            EventListener listener;
            if (events.TryGetValue(key, out listener))
            {
                listener.Invoke();
            }
        }

        public void RemoveEvent(string key, System.Action call)
        {
            EventListener listener;
            if (events.TryGetValue(key, out listener))
            {
                listener.listeners.Remove(call);
            }
        }

        /// <summary> 이벤트 등록 </summary>
        public void AddEvent<T>(string key, System.Action<T> call)
        {
            EventListener listener;
            if (events.TryGetValue(key, out listener))
            {
                if (!listener.listeners.Contains(call))
                {
                    listener.listeners.Add(call);
                }
            }
            else
            {
                listener = new EventListener();
                listener.listeners.Add(call);
                events.Add(key, listener);
            }
        }

        /// <summary> 파라미터 있는 함수의 호출 </summary>
        public void InvokeEvent<T>(string key, T arg)
        {
            EventListener listener;
            if (events.TryGetValue(key, out listener))
            {
                listener.Invoke(arg);
            }
        }

        /// <summary> 이벤트 삭제 </summary>
        public void RemoveEvent<T>(string key, System.Action<T> call)
        {
            EventListener listener;
            if (events.TryGetValue(key, out listener))
            {
                listener.listeners.Remove(call);
            }
        }

        /// <summary> Key 해당 이벤트 전체 제거 </summary>
        public void ClearEvent(string key)
        {
            if (events.ContainsKey(key))
            {
                events[key].Clear();
            }
        }
    }
}