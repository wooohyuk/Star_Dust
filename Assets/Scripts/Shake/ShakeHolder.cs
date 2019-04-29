using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Shake
{
	public class ShakeHolder
	{
		public ShakeInstance ShakeInstance { get; private set; }
		private readonly float FadeOutTime;
		public ShakeHolder(ShakeInstance shakeInstance, float fadeOutTime)
		{
			this.ShakeInstance = shakeInstance;
			this.FadeOutTime = fadeOutTime;
		}

		public void Release(bool doFadeOut = true)
		{
			if(ShakeInstance == null)
			{
				return;
			}
			if(ShakeInstance.CurrentState == ShakeState.Inactive)
			{
				ShakeInstance = null;
				return;
			}

			if(doFadeOut == true)
			{
				ShakeInstance.StartFadeOut(FadeOutTime);
			}
			else
			{
				ShakeInstance.StartFadeOut(0f);
			}

			ShakeInstance = null;
		}
	}
}