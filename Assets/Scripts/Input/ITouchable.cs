using UnityEngine;

namespace Input
{
	public interface ITouchable
	{
		void OnPressDown();
		void OnPressUp();
		void OnStartDrag(Vector3 position);
		void OnDrag(Vector3 position, Vector3 direction);
		void OnEndDrag();
	}
}