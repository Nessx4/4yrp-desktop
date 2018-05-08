/*	NameDialog encapsulates the UI dialog asking the user to name their level.
 */
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameDialog : MonoBehaviour 
{
	[SerializeField] 
	private List<NameDialogButton> buttons;

	[SerializeField]
	private InputField nameInput;

	[SerializeField]
	private InputField descriptionField;

	public bool playMode { private get; set; }

	private void Awake()
	{
		// Assign each button's reference back to this instance.
		foreach(var button in buttons)
			button.nameDialog = this;
	}

	// The Confirm or Cancel button has been pressed.
	public void TakeAction(NameDialogAction action)
	{
		switch(action)
		{
			case NameDialogAction.CANCEL:
				gameObject.SetActive(false);
				break;
			case NameDialogAction.CONFIRM:
				string name = nameInput.text;
				string desc = descriptionField.text;
				LevelManagement.instance.Save(name, desc);

				if (playMode)
					SceneManager.LoadScene("sc_PlayMode");

				gameObject.SetActive(false);
				break;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
			LevelManagement.instance.DropTable();
	}
}
