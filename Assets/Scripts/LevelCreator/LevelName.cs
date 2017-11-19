using System.Collections;
using System.Collections.Generic;

using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class LevelName : MonoBehaviour 
{
	private InputField input;

	private Regex validate = new Regex("[^a-zA-Z0-9 ]");

	private void Start()
	{
		input = GetComponent<InputField>();
	}

	public void UpdateInput()
	{
		string text = input.text;
		text = validate.Replace(text, "");
		input.text = text;
	}
}
