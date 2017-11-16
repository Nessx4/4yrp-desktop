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

	public void Toggle(bool rightDir)
	{
		index = rightDir ? ++index : --index;
		index = Mathf.Clamp(index, 0, toggleValues.Length - 1);

		toggleText.text = toggleValues[index];
	}
}
