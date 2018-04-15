using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class ToolbarButton : MonoBehaviour 
{
	public Toolbar toolbar { protected get; set; }
	private bool canPress = true;

	public void Press()
	{
		if(canPress)
			DoAction();
	}

	protected abstract void DoAction();
}
