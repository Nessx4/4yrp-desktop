using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ThemeButton : MonoBehaviour 
{
	public Themebar  themebar  { private get; set; }
	public ThemeType themeType { private get; set; }

	public Button button { get; private set; }

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	public void Press()
	{
		themebar.ChangeTheme(themeType);
	}
}
