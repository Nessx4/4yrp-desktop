using System.Collections;
using System.Collections.Generic;

using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class LevelName : MonoBehaviour 
{
	// The maximum number of characters for the name.
	[SerializeField] 
	private int charLimit;

	// The level name when this level was loaded.
	private string levelLoaded;

	[SerializeField]
	private InputField input;

	private Regex validate = new Regex("[^a-zA-Z0-9 ]");

	// Counts how many characters you have remaining.
	[SerializeField]
	private Text counter;

	private void Start()
	{
		input.characterLimit = charLimit;
	}

	private void Update()
	{
		counter.gameObject.SetActive(input.isFocused);

		counter.text = (charLimit - input.text.Length).ToString();
	}

	public void UpdateInput()
	{
		string text = input.text;
		text = validate.Replace(text, "");
		input.text = text;
	}

	public string GetName()
	{
		return input.text;
	}

	public void SetName(string name)
	{
		input.text = name;
	}
}
