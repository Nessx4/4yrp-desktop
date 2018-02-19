using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	private string _filename;
	public string filename { 
		get
		{
			Destroy(gameObject, Time.deltaTime);
			return _filename;
		} 
		set
		{
			_filename = value;
		} 
	}

	public static LevelLoader instance { get; private set; }

	public void Start()
	{
		if(instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}
}
