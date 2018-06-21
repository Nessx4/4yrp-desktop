using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
	public static Camera mainCamera { get; set; }

	public static ConnectionManager manager { get; set; }

	private static string currentScene = "";

	private static IEnumerator LoadSceneAsync(string sceneToLoad)
	{
		if(Application.CanStreamedLevelBeLoaded(sceneToLoad))
		{
			if(currentScene != "")
			{
				Debug.Log("Unloading scene: " + currentScene);
				SceneManager.UnloadScene(currentScene);
			}

			var thing = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
			yield return thing;
			currentScene = sceneToLoad;
			Debug.Log(currentScene);
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
		}
		else
			Debug.LogWarning("Requested scene does not exist.");
	}

	public static void LoadScene(string sceneToLoad)
	{
		manager.StartCoroutine(LoadSceneAsync(sceneToLoad));
	}

	public static Vector3 CursorToWorldPoint(Vector2 cursorPoint)
	{
		Vector3 pos = mainCamera.ViewportToWorldPoint(cursorPoint);

		return pos;
	}
}
