using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Scene
{
	public enum SceneType
	{
		Undefined = 0,
		Intro,
		Title,
		Credit,
		InGame,
	}
	public class SceneTransaction : MonoBehaviour
	{
		public static SceneTransaction Instance;

		[SerializeField]
		private List<AbstractScene> _scenes = new List<AbstractScene>();

		private AbstractScene _currentScene;

		private void Awake()
		{
			Instance = this;
			Common.StaticInfo.StaticInfoManager.Instance.Init("StaticData/Common/");
		}

		private void Start()
		{
			TransactionTo(SceneType.Intro);
		}
	
		public void TransactionTo(SceneType type)
		{
			AbstractScene nextScene = _scenes.Find(s => s.Type == type);

			if (_currentScene != null)
			{
				_currentScene.Exit(nextScene);
			}

			nextScene.Enter(_currentScene);
			_currentScene = nextScene;
		}
	}
}