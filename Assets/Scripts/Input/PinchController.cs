using UnityEngine;

namespace Input
{
	public class PinchController : MonoBehaviour
	{
		[SerializeField]
		private InputController _inputController;

		private void Awake()
		{
			_inputController.OnPinch += OnPinch;
		}

		private void OnPinch(float delta)
		{
		}
	}
}