using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ButtonRemap : MonoBehaviour 
{
	[SerializeField]
	private Text keyText;

	private KeyCode currKey;

	private bool isActive = false;

	private void Update()
	{
		if(isActive)
		{
			foreach(KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
			{
				if(Input.GetKeyDown(k))
				{
					keyText.text = k.ToString();
					currKey = k;
					isActive = false;

					OptionsMenu.menu.RemoveActiveRemap();
				}
			}
		}
	}

	public void Activate()
	{
		if(OptionsMenu.menu.SetActiveRemap(this))
			isActive = true;
	}
}
