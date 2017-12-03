using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class BrowserButton : MonoBehaviour
{
	[SerializeField]
	private LevelLoader loader;

	private string levelName;

	// Load the Level Editor without needing to load a new level.
	public void NewLevel()
	{
		SceneManager.LoadScene("sc_LevelCreator");
	}

	// When we reach the Level Editor, we will try to load a level.
	public void LoadLevel()
	{
		LevelLoader loaderObj = Instantiate(loader, null);
		DontDestroyOnLoad(loaderObj.gameObject);
		loaderObj.SetLevel(levelName);

		SceneManager.LoadScene("sc_LevelCreator");
	}

	// Set the path name of the level we will load.
	public void SetLevel(string levelName)
	{
		this.levelName = levelName;
	}
}
