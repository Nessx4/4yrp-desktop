using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuButton : MonoBehaviour 
{
	[SerializeField] 
	private RectTransform mainMenuRoot;

	[SerializeField] private Image darkenImage;

	public void LoadLevel(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}

	public void OpenOptions()
	{
		StartCoroutine(ChangeRootShape(0.75f));
		StartCoroutine(ChangeBackgroundDarkness(0.9f));
	}

	private IEnumerator ChangeRootShape(float targetSize)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float startSize = mainMenuRoot.localScale.x;

		for(float x = 0.0f; x <= 0.125f; x += Time.deltaTime)
		{
			float newScale = Mathf.Lerp(startSize, targetSize, x * 8.0f);
			mainMenuRoot.localScale = new Vector3(newScale, newScale, newScale);
			yield return wait;
		}

		mainMenuRoot.localScale = new Vector3(targetSize, targetSize, targetSize);
	}

	private IEnumerator ChangeBackgroundDarkness(float targetAlpha)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float startAlpha = darkenImage.color.a;

		for(float x = 0.0f; x <= 0.125f; x += Time.deltaTime)
		{
			Color col = darkenImage.color;
			col.a = Mathf.Lerp(startAlpha, targetAlpha, x * 8.0f);
			darkenImage.color = col;
			yield return wait;
		}

		Color col2 = darkenImage.color;
		col2.a = targetAlpha;
		darkenImage.color = col2;
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
