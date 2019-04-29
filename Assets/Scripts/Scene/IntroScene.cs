using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Scene
{
	public class IntroScene : AbstractScene
	{
		[SerializeField]
		private SkeletonAnimation _introAnimation;

		private bool _canChangeScene;

		public override void Enter(AbstractScene beforeScene)
		{
			base.Enter(beforeScene);

			_introAnimation.gameObject.SetActive(true);
			_introAnimation.AnimationState.Complete += OnAnimationStateComplete;
			StartCoroutine(SoundRoutine());
		}

		private IEnumerator SoundRoutine()
		{
			yield return new WaitForSeconds(0.5f);
			SoundManager.Instance.Play("Sounds/BigBang");
			yield return new WaitForSeconds(5.5f);
			SoundManager.Instance.Play("Sounds/Un_BigBang");
			
		}

		private void OnAnimationStateComplete(TrackEntry trackentry)
		{
			_introAnimation.AnimationState.End -= OnAnimationStateComplete;
			_introAnimation.state.SetAnimation(0, "idle", true);
			_canChangeScene = true;
		}

		private void Update()
		{
			if (IsActiveScene == true && _canChangeScene == true)
			{
				_canChangeScene = false;
				SceneTransaction.Instance.TransactionTo(SceneType.Title);
			}
		}

		public override void Exit(AbstractScene nextScene)
		{
			base.Exit(nextScene);
//			_introAnimation.gameObject.SetActive(false);
		}
	}
}