using System.Collections.Generic;

public class PoolManager<T> where T : IPoolable
{
	public static PoolManager<T> Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PoolManager<T>();
			}

			return _instance;
		}
	}

	private static PoolManager<T> _instance;
	private Dictionary<string,List<T>> _pool = new Dictionary<string, List<T>>();

	public T Get(string id)
	{
		if (_pool.ContainsKey(id) == false)
		{
			_pool.Add(id, new List<T>());
		}

		List<T> targetPool = _pool[id];
		if (targetPool.Count == 0)
		{
			T newInstance = ObjectFactory<T>.Instance.Create(id);
			targetPool.Add(newInstance);
		}

		T target = targetPool[targetPool.Count - 1];
		targetPool.RemoveAt(targetPool.Count - 1);
		target.OnInit();

		return target;
	}

	public void Release(T instance, string id)
	{
		instance.OnRelease();
		_pool[id].Add(instance);
	}
}

public class SimplePool<T> where T : IPoolable
{
	private List<T> _pool = new List<T>();
	private System.Func<T> _createFunc;

	public SimplePool(System.Func<T> createFunc)
	{
		_createFunc = createFunc;
	}

	public T Get()
	{
		if (_pool.Count == 0)
		{
			T newInstance = _createFunc();
			_pool.Add(newInstance);
		}

		T target = _pool[_pool.Count - 1];
		_pool.RemoveAt(_pool.Count - 1);
		target.OnInit();

		return target;
	}

	public void Release(T instance)
	{
		instance.OnRelease();
		_pool.Add(instance);
	}
}