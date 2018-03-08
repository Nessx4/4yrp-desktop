using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class BrowserButton : MonoBehaviour
{
	[SerializeField]
	private LevelLoader loader;
	
	public string filename { private get; set; }
	public string description { private get; set; }

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
		loaderObj.filename = filename;
		loaderObj.description = description;

		SceneManager.LoadScene("sc_LevelCreator");
	}
}
