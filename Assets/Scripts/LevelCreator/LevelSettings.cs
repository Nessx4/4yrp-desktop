using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
	private bool visible = false;

	public static LevelSettings settings;

	private void Start()
	{
		settings = this;
		gameObject.SetActive(false);
	}

	public void ToggleVisible()
	{
		gameObject.SetActive(visible = !visible);
	}
}
