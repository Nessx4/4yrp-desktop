/*	NameDialog encapsulates the UI dialog asking the user to name their level.
 */
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class NameDialog : MonoBehaviour 
{
	[SerializeField] 
	private List<NameDialogButton> buttons;

	[SerializeField]
	private InputField nameInput;

	[SerializeField]
	private InputField descriptionField;

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
				break;
			case NameDialogAction.CONFIRM:
				break;
		}
	}
}
