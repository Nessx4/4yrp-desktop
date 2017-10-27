using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour 
{
	[SerializeField] private Text text;
	[SerializeField] private Image icon;

	public void SetText(string message)
	{
		text.text = message;
	}

	public void SetIcon(Sprite img)
	{
		icon.sprite = img;
	}

	public void Close()
	{
		Destroy(gameObject);
	}
}
