using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
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

	private Image image;

	private void Start()
	{
		image = GetComponent<Image>();
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

	public void Show()
	{
		image.color = showColor;
	}

	public void Hide()
	{
		image.color = hideColor;
	}
}
