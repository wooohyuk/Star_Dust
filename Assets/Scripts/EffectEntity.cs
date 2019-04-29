using System.Collections;
using UnityEngine;

public class EffectEntity : MonoBehaviour, IPoolable
{
	public string Path;
	[SerializeField]
	private float _time;

	public event System.Action<EffectEntity> OnFinished;

	private ParticleSystem _particleSystem;

	private void Awake()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}

	public void OnInit()
	{
		gameObject.SetActive(true);
		_particleSystem.Play();
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
		transform.SetParent(null);
		gameObject.SetActive(false);
	}
}
