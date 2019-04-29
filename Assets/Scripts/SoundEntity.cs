using UnityEngine;
using System.Collections;

public class SoundEntity : MonoBehaviour, IPoolable
{
	public string Path;
	[SerializeField]
	private float _time;

	public event System.Action<SoundEntity> OnFinished;

	private AudioSource _audioSource;

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	public void OnInit()
	{
		_audioSource.Play();
		gameObject.SetActive(true);
		if (_time > 0)
		{
			StartCoroutine(DelayEnd());
		}
	}

	private IEnumerator DelayEnd()
	{
		yield return new WaitForSeconds(_time);
		OnFinished?.Invoke(this);
	}

	public void OnRelease()
	{
		_audioSource.Stop();
		transform.SetParent(null);
		gameObject.SetActive(false);
	}
}
