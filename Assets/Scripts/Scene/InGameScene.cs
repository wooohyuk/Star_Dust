using System.Collections;
using UnityEngine;

namespace Scene
{
	public class InGameScene : AbstractScene
	{
		public override void Enter(AbstractScene beforeScene)
		{
			base.Enter(beforeScene);
			GameManager.Instance.StartGame();
			Camera.main.GetComponent<Animator>().Play("GameStart");
			StartCoroutine(WaitAndDisable());
		}

		private IEnumerator WaitAndDisable()
		{
			yield return new WaitForSeconds(3f);
			Camera.main.GetComponent<Animator>().enabled = false;
		}
    }
}