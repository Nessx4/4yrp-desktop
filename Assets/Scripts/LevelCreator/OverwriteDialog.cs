/*	This dialog appears when a user tries to save an existing level.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class OverwriteDialog : MonoBehaviour
{
	// The message that asks users if they wish to overwrite a level.
	[SerializeField]
	private Text levelMessage;

	private void Start()
	{
		gameObject.SetActive(false);
	}

	public void SetLevelName(string name)
	{
		levelMessage.text = "The level \"" + name + "\" already exists. Do you wish to overwrite it?";
	}

	public void Accept()
	{
		//LevelEditor.instance.Save(true);
		throw new System.NotImplementedException();
		gameObject.SetActive(false);
	}

	public void Reject()
	{
		gameObject.SetActive(false);
	}
}
