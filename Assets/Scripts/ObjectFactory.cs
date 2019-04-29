using System;
using System.Collections.Generic;
using UnityEngine;
using Logic.Entity;
using Utility;


public class ObjectFactory<T>
{
	public static ObjectFactory<T> Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ObjectFactory<T>();
			}

			return _instance;
		}
	}

	private static ObjectFactory<T> _instance;
	private Dictionary<string, GameObject> resources = new Dictionary<string, GameObject>(); 

	public T Create(string id)
	{
		if (id.Contains("Star_"))
		{
			id = "Star";
		}
		if (resources.ContainsKey(id) == false)
		{
			string resourcePath = $"Prefabs/{id}";
			GameObject resource = Resources.Load<GameObject>(resourcePath);
			resources.Add(id, resource);
		}
		return GameObject.Instantiate(resources[id]).GetComponent<T>();

		throw new NotSupportedException();
	}
}