using UnityEngine;
using Spine;
using Spine.Unity;

namespace Scene
{
	public class CreditScene : AbstractScene
	{
		[SerializeField]
		private SkeletonAnimation _animation;

		private bool _canChangeScene;

		public override void Enter(AbstractScene beforeScene)
		{
			base.Enter(beforeScene);

			_animation.gameObject.SetActive(true);
			_canChangeScene = true;
		}

		private void Update()
		{
			if (UnityEngine.Input.GetMouseButtonDown(0) == true && _canChangeScene == true)
			{
				_canChangeScene = false;
				SceneTransaction.Instance.TransactionTo(SceneType.Title);
			}
		}

		public override void Exit(AbstractScene nextScene)
		{
			base.Exit(nextScene);
			_animation.gameObject.SetActive(false);
		}
	}
}