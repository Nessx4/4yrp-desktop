using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour 
{
	[SerializeField] 
	private Text toggleText;

	[SerializeField] 
	private string[] toggleValues;

	[SerializeField]
	private int index = 0;

	// Set the string if it is a valid message string.
	public void SetString(string txt)
	{
		for(int i = 0; i < toggleValues.Length; ++i)
		{
			if(toggleValues[i]  == txt)
			{
				index = i;
				toggleText.text = txt;

				return;
			}
		}
	}

	public void SetIndex(int index)
	{
		index = Mathf.Clamp(index, 0, toggleValues.Length - 1);

		toggleText.text = toggleValues[index];
	}

	public int GetIndex()
	{
		return index;
	}

	public void Toggle(bool rightDir)
	{
		index = rightDir ? ++index : --index;
		index = Mathf.Clamp(index, 0, toggleValues.Length - 1);

		toggleText.text = toggleValues[index];
	}
}
