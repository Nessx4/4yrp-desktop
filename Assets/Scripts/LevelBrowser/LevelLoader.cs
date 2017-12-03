using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	private string levelName;

	public static LevelLoader loader;

	public void Start()
	{
		loader = this;
	}

	public void SetLevel(string levelName)
	{
		this.levelName = levelName;
	}

	public string GetLevel()
	{
		Invoke("DestroyObj", Time.deltaTime);
		return levelName;
	}

	private void DestroyObj()
	{
		Destroy(gameObject);
	}
}
