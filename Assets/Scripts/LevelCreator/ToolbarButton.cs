using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

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
		TilePlacement.placement.Undo();
	}

	public void Redo()
	{
		TilePlacement.placement.Redo();
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
		TilePlacement.placement.Clear();
	}

	public void Menu()
	{
		LevelSettings.settings.ToggleVisible();
	}

	public ToolType GetTool()
	{
		return tool;
	}

	public void Press()
	{
		anim.SetTrigger(isShown ? "Press" : "Shake");
	}

	public void Show()
	{
		image.color = showColor;
		isShown = true;
	}

	public void Hide()
	{
		image.color = hideColor;
		isShown = false;
	}
}
