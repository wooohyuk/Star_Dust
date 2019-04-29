using System.Collections;
using Logic.Entity;
using Logic.Situation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	public bool IsGameProcessing { get; private set; }
	public override void Init()
	{
	}

	public void StartGame()
	{
		IsGameProcessing = true;
		var firstPlanetCreation = new CreatePlanetSituation(Vector3.zero);

        SoundManager.Instance.Play("Sounds/Main_Theme");
        if (Testment.testment != null && Testment.testment.isTest == true && false)
		{
			GameObject.FindObjectOfType<DustGenerator>().Do();
		}
	}

	private void Update()
	{
		if (IsGameProcessing == true && UnityEngine.Input.GetKeyDown(KeyCode.Escape) == true)
		{
			ResetGame();
		}
	}

	public void ResetGame()
	{
		SceneManager.LoadScene(0);
		//
		StartCoroutine(ResetGameProcess());
	}

	private IEnumerator ResetGameProcess()
	{
		IsGameProcessing = false;
		Animator animator = Camera.main.GetComponent<Animator>();
		animator.enabled = true;
		animator.Play("GameReset");
		FindObjectOfType<UIManager>().PlayResetAnimation();
		yield return new WaitForSeconds(1f);
		animator.enabled = false;
		EntityManager.Instance.DestroyAll();
		IsGameProcessing = true;
	}
	

	public void EndGame()
	{
		IsGameProcessing = false;
	}
}
