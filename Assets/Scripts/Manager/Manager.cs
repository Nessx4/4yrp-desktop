using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public static class Manager
{
	private static string currentScene = "";

	public static void LoadScene(string sceneToLoad)
	{
		if(Application.CanStreamedLevelBeLoaded(sceneToLoad))
		{
			if(currentScene != "")
				SceneManager.UnloadScene(currentScene);

			SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
			currentScene = sceneToLoad;
		}
		else
			Debug.LogWarning("Requested scene does not exist.");
	}
}
