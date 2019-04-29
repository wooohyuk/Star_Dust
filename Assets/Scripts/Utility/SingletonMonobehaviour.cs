using UnityEngine;

namespace Utility
{
	public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType(typeof(T)) as T;

					if (_instance == null)
					{
						GameObject go = new GameObject(typeof(T).ToString());
						Debug.Log($"Singleton {go.name} Created.");
						_instance = go.AddComponent<T>();
						DontDestroyOnLoad(go);
					}
				}
				return _instance;
			}
		}

		public static bool IsInit
		{
			get { return _instance != null; }
		}

		public virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this as T;
				if (_instance != null)
				{
					_instance.gameObject.hideFlags = HideFlags.DontSave;
				}
                
				Init();
			}
			else
			{
				Debug.Log(typeof(T).ToString() + "  is Duplicate");
				Destroy(gameObject);
			}
		}

		public abstract void Init();

		public virtual void OnApplicationQuit()
		{
			_instance = null;
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}
	}
}