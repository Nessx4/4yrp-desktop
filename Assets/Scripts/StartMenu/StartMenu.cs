using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
	public void DesignLevels_Btn()
	{

	}

	public void PlayLevels_Btn()
	{

	}

	public void Options_Btn()
	{

	}

	public void Quit_Btn()
	{
		Process.Start("Chrome.exe", "http://www.google.com");
		Application.Quit();
	}
}