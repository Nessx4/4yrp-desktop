using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour 
{
	public void MakeLevels()
	{
		SceneManager.LoadScene("sc_LevelEditor");
	}

	public void PlayLevels()
	{
		SceneManager.LoadScene("");
	}

	public void Options()
	{

	}

	public void Quit()
	{

	}
}
