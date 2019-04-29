using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Canvas _canvas;

	private void Awake()
	{
		
	}

	public void PlayResetAnimation()
	{
		_canvas.GetComponent<Animator>().Play("UIReset");
	}
}
