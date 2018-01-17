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

	[SerializeField]
	private Color showColor;

	[SerializeField]
	private Color hideColor;

	[SerializeField]
	private LevelLoader loader;

	private bool isShown = true;

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
		TileDrawWrapper.Get().Undo(0);
	}

	public void Redo()
	{
		TileDrawWrapper.Get().Redo(0);
	}

	public void Save()
	{
		LevelEditor.editor.Save(false);
	}

	public void SetTool()
	{
		if(isShown)
			toolbar.SetTool(this);
	}

	public void Clear()
	{
		TileDrawWrapper.Get().Clear(0);
	}

	public void Menu()
	{
		LevelSettings.settings.ToggleVisible();
	}

	public void Play()
	{
		LevelEditor.editor.Save(true);

		LevelLoader loaderObj = Instantiate(loader, null);
		DontDestroyOnLoad(loaderObj.gameObject);
		loaderObj.SetLevel(LevelEditor.editor.GetLevelName());

		SceneManager.LoadScene("sc_LevelRuntime");
	}

	public ToolType GetTool()
	{
		return tool;
	}

	public void Press()
	{
		anim.SetTrigger(isShown ? "Press" : "Shake");
	}

	public void SetVisible(bool visible)
	{
		if(visible)
		{
			image.color = showColor;
			isShown = true;
		}
		else
		{
			image.color = hideColor;
			isShown = false;
		}
	}
}
