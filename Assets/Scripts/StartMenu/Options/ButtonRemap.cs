using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ButtonRemap : MonoBehaviour 
{
	public Text buttonName;

	// The key mapped to the button name.
	[SerializeField]
	private Text activeKey;

	[SerializeField]
	private Image bgImage;

	[SerializeField]
	public Button btn;

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
					activeKey.text = k.ToString();
					currKey = k;

					Deactivate();
				}
			}
		}
	}

	public void Activate()
	{
		if(OptionsMenu.menu.SetActiveRemap(this))
		{
			bgImage.color = new Color(0.5f, 0.5f, 0.25f, 1.0f);
			isActive = true;
		}
	}

	public void Deactivate()
	{
		bgImage.color = new Color(0.25f, 0.25f, 0.25f, 1.0f);

		isActive = false;
		OptionsMenu.menu.RemoveActiveRemap();
	}

	public KeyCode GetCode()
	{
		return currKey;
	}

	public void SetValues(string name, KeyCode key)
	{
		buttonName.text = name;
		activeKey.text = key.ToString();
		currKey = key;
	}
}
