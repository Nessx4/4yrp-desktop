/*	ToolbarButton denotes one button on the Toolbar.
 */
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class ToolbarButton : MonoBehaviour
{
	[SerializeField]
	private Toolbar toolbar;

	[SerializeField]
	private ToolType tool;
	public ToolType Tool 
	{ 
		get { return tool; }
		private set { tool = value; } 
	}

	private Color showColor = Color.white;
	private Color hideColor = new Color(0.29f, 0.24f, 0.42f, 1.0f);

	[SerializeField]
	private LevelLoader loader;

	private bool _isShown = true;
	public bool IsShown
	{
		get
		{
			return _isShown;
		}
		set
		{
			_isShown = value;
			if(value)
			{
				if(image.color == hideColor)
					image.color = showColor;
			}
			else
				image.color = hideColor;
		}
	}

	[SerializeField] 
	private SaveDialog saveDialog;

	private Image image;
	private Animator anim;

	private void Start()
	{
		image = GetComponent<Image>();
		anim = GetComponent<Animator>();
	}

	public void SetColor(Color newColor)
	{
		image.color = newColor;
	}
	
	public void Undo()
	{
		CreatorPlayerWrapper.Get().Undo(0);
	}

	public void Redo()
	{
		CreatorPlayerWrapper.Get().Redo(0);
	}

	public void Save()
	{
		//LevelEditor.instance.Save(false);
		saveDialog.Open();
	}

	public void SetTool()
	{
		if(IsShown)
			toolbar.SetTool(this, true);
	}

	public void Clear()
	{
		CreatorPlayerWrapper.Get().Clear(0);
	}

	public void Menu()
	{
		LevelSettings.settings.ToggleVisible();
	}

	public void Play()
	{
		//LevelEditor.instance.Save(true);
		saveDialog.Open();
		/*
		LevelLoader loaderObj = Instantiate(loader, null);
		DontDestroyOnLoad(loaderObj.gameObject);
		loaderObj.filename = LevelEditor.instance.filename;

		SceneManager.LoadScene("sc_LevelRuntime");
		*/
	}

	public void Press()
	{
		anim.SetTrigger(IsShown ? "Press" : "Shake");
	}
}

public enum ToolType
{
	SAVE, UNDO, REDO, PENCIL, ERASER, GRAB, FILL, RECT_HOLLOW, RECT_FILL, 
	CLEAR, MENU, PLAY
}
