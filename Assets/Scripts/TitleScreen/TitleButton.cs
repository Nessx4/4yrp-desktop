using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TitleButton : MonoBehaviour 
{
	public void MakeLevels()
	{
		Manager.LoadScene("sc_LevelEditor");
	}

	public void PlayLevels()
	{
		Manager.LoadScene("");
	}

	public void Options()
	{

	}

	public void Quit()
	{

	}
}
