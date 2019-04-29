using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Scene
{
	public class TitleScene : AbstractScene
	{

        [SerializeField]
		private SkeletonAnimation _introAnimation;

		private bool _canChangeScene = true;

        float aa;
        int a = 0;

        private void Awake()
		{
            
        }

      

        public void Update()
		{
			if (IsActiveScene == true && _canChangeScene == true)
			{
				_canChangeScene = false;
				_introAnimation.state.SetAnimation(0, "outro", true);
				_introAnimation.AnimationState.Complete += OnAnimationStateComplete;
			}
		}
		
		private void OnAnimationStateComplete(TrackEntry trackentry)
		{
			_canChangeScene = true;
			_introAnimation.AnimationState.Complete -= OnAnimationStateComplete;
			_introAnimation.gameObject.SetActive(false);
			SceneTransaction.Instance.TransactionTo(SceneType.InGame);
		}

		public override void Enter(AbstractScene beforeScene)
		{
			base.Enter(beforeScene);

//			_canvas.gameObject.SetActive(true);
		}

		public override void Exit(AbstractScene nextScene)
		{
			base.Exit(nextScene);

//			_canvas.gameObject.SetActive(false);
		}
	}
}