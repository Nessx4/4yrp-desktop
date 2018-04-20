using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TestButton : MonoBehaviour 
{
	public void Press()
	{
		LevelManagement.instance.Save();
	}
}
