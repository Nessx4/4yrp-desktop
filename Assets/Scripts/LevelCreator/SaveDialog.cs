/*	When the Save button is pressed, 
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SaveDialog : MonoBehaviour 
{
	[SerializeField]
	private InputField nameInput;

	public void Open()
	{
		gameObject.SetActive(true);
		nameInput.text = LevelEditor.instance.levelName;
	}

	public void Accept()
	{
		if(nameInput.text == "")
		{
			WarningMessage.instance.SetMessage(true, "Name your level!");
			return;
		}

		LevelEditor.instance.Save(nameInput.text);
	}

	public void Cancel()
	{
		gameObject.SetActive(false);
	}
}
