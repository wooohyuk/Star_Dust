using System.Collections.Generic;
using UnityEngine;

namespace Input
{
	using UInput = UnityEngine.Input;
	public class InputController : MonoBehaviour
	{
		[SerializeField]
		private float _slideTouchArea;
	
		private enum InputState
		{
			None = 0,
			Press, // 가만히 누르고 기다리는 상태
			Sliding,
			Up,
			Pinch,
		}

		private InputState _state;
		private InputState _prevState;
		private List<Touch> _prevTouches = new List<Touch>();

		public event System.Action<Vector3> OnPressStart;
		public event System.Action<Vector3> OnPressUp;
		public event System.Action<Vector3> OnTouchUp;
		public event System.Action<Vector3> OnClick;
		public event System.Action<Vector3, Vector3> OnSlide;
		public event System.Action<Vector3> OnTouchPosChange;
		public event System.Action<float> OnPinch;

		private float _pressDownTime;
		private void Update()
		{
			if (GameManager.Instance.IsGameProcessing == false)
			{
				_prevState = InputState.None;
				return;
			}

			List<Touch> touches = InputHelper.GetTouches();
			if (touches.Count == 0)
			{
				_state = InputState.None;
				return;
			}

			if (touches.Count == 0)
			{
				_state = InputState.None;
			}
			else if (touches.Count == 1)
			{
				if (touches[0].phase == TouchPhase.Began)
				{
					_pressDownTime = Time.time;
				}

				OnTouchPosChange?.Invoke(touches[0].position);
				float deltaPositionLength = touches[0].deltaPosition.magnitude;
				float moveSpeed = deltaPositionLength * Time.deltaTime;
				if (touches[0].phase == TouchPhase.Ended)
				{
					if (Time.time - _pressDownTime < 0.4f)
					{
						OnClick?.Invoke(touches[0].position);
					}
					OnPressUp?.Invoke(touches[0].position);
					_state = InputState.Up;
				}
				else if (moveSpeed > 0.1f)
				{
					_state = InputState.Sliding;
				}
				else
				{
					_state = InputState.Press;
				}
			}
			else if (touches.Count == 2)
			{
				_state = InputState.Pinch;
			}
			

			ProcessInput(touches, _state);
//			Debug.Log($"CurrentState : {_state}");
			_prevState = _state;
			_prevTouches = touches;
		}

		private void ProcessInput(List<Touch> touches, InputState state)
		{
			switch (state)
			{
				case InputState.None:
					break;
				case InputState.Press:
					if (touches[0].phase == TouchPhase.Began)
					{
						OnPressStart?.Invoke(touches[0].position);
					}
					break;
				case InputState.Sliding:
					Vector3 prevPos = _prevTouches[0].position;
					Vector3 currentPos = touches[0].position;
					OnSlide?.Invoke(prevPos, currentPos);
					break;
				case InputState.Up:
					OnTouchUp?.Invoke(touches[0].position);
					break;
				case InputState.Pinch:
					if (_prevState != InputState.Pinch)
					{
						return;
					}

					float prevLength = (_prevTouches[0].position - _prevTouches[1].position).magnitude;
					float currentLength = (touches[0].position - touches[1].position).magnitude;
					OnPinch?.Invoke(Mathf.Abs(prevLength - currentLength));
					break;
				default:
					break;
			}
		}
	}
}



















