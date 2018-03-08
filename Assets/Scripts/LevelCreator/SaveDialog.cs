/*	When the Save button is pressed, this save dialog is invoked.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SaveDialog : MonoBehaviour 
{
	[SerializeField]
	private InputField nameInput;

	[SerializeField] 
	private InputField descInput;

	private void Awake()
	{
		gameObject.SetActive(false);
	}

	public void Open()
	{
		gameObject.SetActive(true);
		nameInput.text = LevelEditor.instance.levelName;
		descInput.text = LevelEditor.instance.description;
	}

	public void Accept()
	{
		if(nameInput.text == "")
		{
			WarningMessage.instance.SetMessage(true, "Name your level!");
			return;
		}

		LevelEditor.instance.Save(nameInput.text, descInput.text);
	}

	public void Cancel()
	{
		gameObject.SetActive(false);
	}
}
