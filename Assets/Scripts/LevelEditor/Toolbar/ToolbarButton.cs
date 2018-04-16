using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class ToolbarButton : MonoBehaviour 
{
	public Toolbar toolbar { protected get; set; }
	private bool canPress = true;

	private void Start()
	{
		GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-5, 5)));
	}

	public void Press()
	{
		if(canPress)
			DoAction();
	}

	protected abstract void DoAction();
}
