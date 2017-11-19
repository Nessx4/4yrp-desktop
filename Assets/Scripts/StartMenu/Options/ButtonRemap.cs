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

	private Color activeColor = new Color(0.5f, 0.5f, 0.25f, 1.0f);
	private Color inactiveColor = new Color(0.25f, 0.25f, 0.25f, 0.5f);
	private Color invalidColor = new Color(0.5f, 0.25f, 0.25f, 0.5f);

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
					OptionsMenu.menu.RemoveActiveRemap();
				}
			}
		}
	}

	public void Activate()
	{
		if(OptionsMenu.menu.SetActiveRemap(this))
		{
			bgImage.color = activeColor;
			isActive = true;
		}
	}

	public void Deactivate()
	{
		bgImage.color = inactiveColor;

		isActive = false;
	}

	public void Invalidate()
	{
		bgImage.color = invalidColor;
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
