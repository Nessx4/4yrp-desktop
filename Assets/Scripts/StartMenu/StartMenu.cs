using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class StartMenu : MonoBehaviour
{
	[SerializeField]
	private MenuRoot root;

	[SerializeField]
	private FrostedUI frostUI;
	
	private RectTransform mainMenuRoot;

	private void Start()
	{
		mainMenuRoot = GetComponent<RectTransform>();
	}

	// Fade out and shrink the Start Menu screen.
	public void Minimise()
	{
		StartCoroutine(ChangeStartRootShape(0.75f));
		frostUI.StartCoroutine(frostUI.ChangeColour(0.95f, 4.0f));
	}

	// Fade in and grow the Start Menu screen.
	public void Maximise()
	{
		StartCoroutine(ChangeStartRootShape(1.0f));
		frostUI.StartCoroutine(frostUI.ChangeColour(0.0f, 0.0f));
	}

	// Make the Start Menu smaller or larger.
	private IEnumerator ChangeStartRootShape(float targetSize)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float startSize = mainMenuRoot.localScale.x;

		for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime * 8.0f)
		{
			float newScale = Mathf.Lerp(startSize, targetSize, x);
			mainMenuRoot.localScale = new Vector3(newScale, newScale, newScale);
			yield return wait;
		}

		mainMenuRoot.localScale = new Vector3(targetSize, targetSize, targetSize);
	}

	public void SelectOptions()
	{
		root.ChangeToOptions();
	}

	public void Quit()
	{
		//Process.Start("Chrome.exe", "http://www.google.com");
		Application.Quit();
	}
}