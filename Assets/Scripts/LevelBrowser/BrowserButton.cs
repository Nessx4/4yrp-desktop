using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevelButton : MonoBehaviour
{
	private string levelToLoad;

	public void NewLevel()
	{
		SceneManager.LoadScene("sc_LevelCreator");
	}

	public void LoadLevel()
	{
		Debug.Log("Load level: " + levelToLoad);
	}
}
