using System.Collections.Generic;
using UnityEngine;
using Utility;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
	private Dictionary<string, SimplePool<EffectEntity>> _pools = new Dictionary<string, SimplePool<EffectEntity>>();
	public override void Init()
	{
	}

	public EffectEntity Attach(string path, Transform parent)
	{
		EffectEntity entity = Get(path);
		if (entity == null)
		{
			return null;
		}

		entity.transform.SetParent(parent);
		entity.transform.localPosition = Vector3.zero;
		entity.OnFinished += OnEffectEnded;
		return entity;
	}

	public EffectEntity Attach(string path, Vector3 position)
	{
		EffectEntity entity = Get(path);
		if (entity == null)
		{
			return null;
		}

		entity.transform.SetParent(null);
		entity.transform.position = position;
		entity.OnFinished += OnEffectEnded;
		return entity;
	}

	private EffectEntity Get(string path)
	{
		if (_pools.ContainsKey(path) == false)
		{
			_pools.Add(path, new SimplePool<EffectEntity>(() => Instantiate(Resources.Load<GameObject>(path)).GetComponent<EffectEntity>()));
		}

		EffectEntity entity = _pools[path].Get();
		entity.Path = path;
		return entity;
	}

	private void OnEffectEnded(EffectEntity entity)
	{
		entity.OnFinished -= OnEffectEnded;
		_pools[entity.Path].Release(entity);
	}
}
