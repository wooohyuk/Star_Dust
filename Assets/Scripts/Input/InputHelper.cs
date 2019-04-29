using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Input
{
	public class InputHelper : MonoBehaviour
	{
		private static TouchCreator lastFakeTouch;

		public static List<Touch> GetTouches()
		{
			List<Touch> touches = new List<Touch>();
			touches.AddRange(UnityEngine.Input.touches);
#if UNITY_EDITOR
			if (lastFakeTouch == null) lastFakeTouch = new TouchCreator();
			if (UnityEngine.Input.GetMouseButtonDown(0))
			{
				lastFakeTouch.phase = TouchPhase.Began;
				lastFakeTouch.deltaPosition = new Vector2(0, 0);
				lastFakeTouch.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
				lastFakeTouch.fingerId = 0;
			}
			else if (UnityEngine.Input.GetMouseButtonUp(0))
			{
				lastFakeTouch.phase = TouchPhase.Ended;
				Vector2 newPosition = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
				lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
				lastFakeTouch.position = newPosition;
				lastFakeTouch.fingerId = 0;
			}
			else if (UnityEngine.Input.GetMouseButton(0))
			{
				lastFakeTouch.phase = TouchPhase.Moved;
				Vector2 newPosition = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
				lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
				lastFakeTouch.position = newPosition;
				lastFakeTouch.fingerId = 0;
			}
			else
			{
				lastFakeTouch = null;
			}

			if (lastFakeTouch != null) touches.Add(lastFakeTouch.Create());
#endif


			return touches;
		}
	}
}