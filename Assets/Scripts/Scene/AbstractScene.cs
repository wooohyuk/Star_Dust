using UnityEngine;

namespace Scene
{
	public abstract class AbstractScene : MonoBehaviour
	{
		public SceneType Type => _type;
		[SerializeField]
		private SceneType _type;
		public bool IsActiveScene { get; private set; }

		public virtual void Enter(AbstractScene beforeScene)
		{
			IsActiveScene = true;
			Debug.Log($"Scene entered, {_type}");
		}

		public virtual void Exit(AbstractScene nextScene)
		{
			IsActiveScene = false;
		}
	}
}