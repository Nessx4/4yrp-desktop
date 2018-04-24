using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Themebar : MonoBehaviour 
{
	[SerializeField]
	private ThemeButton themeButtonPrefab;

	[SerializeField]
	private RectTransform themeButtonRoot;

	private Dictionary<ThemeType, Sprite> themeIcons;

	private ThemeType _activeTheme = ThemeType.NONE;

	public ThemeType activeTheme
	{
		get
		{
			return _activeTheme;
		}

		set
		{
			_activeTheme = value;
		}
	}

	public event ThemeChangedEventHandler ThemeChanged;

	public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);

	private void Awake()
	{
		// Register self on the LevelEditor service locator.
		LevelEditor.instance.themebar = this;

		themeIcons = new Dictionary<ThemeType, Sprite>();

		themeIcons.Add(ThemeType.NORMAL, Resources.Load<Sprite>("UI/ThemeIcons/tx_ThemeIcon_Normal"));
		themeIcons.Add(ThemeType.EXAMPLE_THEME_1, Resources.Load<Sprite>("UI/ThemeIcons/tx_ThemeIcon_Eg1"));
		themeIcons.Add(ThemeType.EXAMPLE_THEME_2, Resources.Load<Sprite>("UI/ThemeIcons/tx_ThemeIcon_Eg2"));

		foreach(var themeType in themeIcons.Keys)
		{
			ThemeButton newButton = Instantiate(themeButtonPrefab, themeButtonRoot);
			newButton.themebar = this;
			newButton.themeType = themeType;
			newButton.button.image.sprite = themeIcons[themeType];
		}
	}

	private void Start()
	{
		ChangeTheme(ThemeType.NORMAL);
	}

	public void ChangeTheme(ThemeType themeType)
	{
		if(activeTheme != themeType)
		{
			activeTheme = themeType;
			OnThemeChanged(new ThemeChangedEventArgs(themeType));
		}
	}

	private void OnThemeChanged(ThemeChangedEventArgs e)
	{
		ThemeChangedEventHandler handler = ThemeChanged;

		if(handler != null)
			handler(this, e);
	}

	public ThemeType GetThemeType()
	{
		return activeTheme;
	}
}

public class ThemeChangedEventArgs : EventArgs
{
	public readonly ThemeType themeType;

	public ThemeChangedEventArgs(ThemeType themeType) : base()
	{
		this.themeType = themeType;
	}
}
