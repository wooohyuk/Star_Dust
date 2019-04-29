using UnityEngine;
using Utility;
using System.Collections.Generic;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
	private Dictionary<string, SimplePool<SoundEntity>> _pools = new Dictionary<string, SimplePool<SoundEntity>>();

	public override void Init()
	{
	}

	public SoundEntity Play(string path)
	{
		SoundEntity entity = Get(path);
		if (entity == null)
		{
			return null;
		}

		entity.OnFinished += OnEffectEnded;
		return entity;
	}

	private SoundEntity Get(string path)
	{
		if (_pools.ContainsKey(path) == false)
		{
			_pools.Add(path, new SimplePool<SoundEntity>(() => Instantiate(Resources.Load<GameObject>(path)).GetComponent<SoundEntity>()));
		}

		SoundEntity entity = _pools[path].Get();
		entity.Path = path;
		return entity;
	}

	private void OnEffectEnded(SoundEntity entity)
	{
		entity.OnFinished -= OnEffectEnded;
		_pools[entity.Path].Release(entity);
	}
}
