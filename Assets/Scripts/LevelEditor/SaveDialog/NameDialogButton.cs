/*	NameDialogButton denotes one of two buttons on the Name Dialog UI, where 
 *	each button performs an action; Cancel closes the window, while Confirm
 *	saves the level with the given name. It may also open Play Mode depending
 *	on other factors.
 */
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NameDialogButton : MonoBehaviour 
{
	[SerializeField]
	private NameDialogAction action;

	public NameDialog nameDialog { private get; set; }

	public void Press()
	{
		nameDialog.TakeAction(action);
	}
}
