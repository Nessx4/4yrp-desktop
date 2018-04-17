using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Timerbar : MonoBehaviour 
{
	[SerializeField]
	private Text timerText;

	private int time = 500;

	public void ChangeTime(int amount)
	{
		time = Mathf.Clamp(time + amount, 25, 500);
		timerText.text = time.ToString();
	}
}
