using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour 
{
	[SerializeField]
	private Color activeColor;

	[SerializeField]
	private ToolbarButton activeTool;

	[SerializeField] 
	private List<ToolbarButton> buttons;
	private Dictionary<ToolType, ToolbarButton> toolButtons = 
		new Dictionary<ToolType, ToolbarButton>();

	private void Start()
	{
		activeTool.SetColor(activeColor);

		foreach(var button in buttons)
			toolButtons.Add(button.Tool, button);

		CreatorPlayerWrapper.Get().GetPlayer(0).TileChanged += TileChanged;
		CreatorPlayerWrapper.Get().GetPlayer(0).ToolChanged += ToolChanged;
		CreatorPlayerWrapper.Get().GetPlayer(0).UndoRedo += UndoRedo;
	}

	// Set a tool directly from a toolbar button.
	public void SetTool(ToolbarButton newTool)
	{
		activeTool.SetColor(Color.white);
		activeTool = newTool;
		activeTool.SetColor(activeColor);

		CreatorPlayerWrapper.Get().SetActiveTool(0, activeTool.Tool);
	}

	// For certain buttons, only active if the tile is unit size.
	private void TileChanged(object sender, TileChangedEventArgs e)
	{
		toolButtons[ToolType.FILL].IsShown = (e.tile.IsUnitSize());
		toolButtons[ToolType.RECT_HOLLOW].IsShown = (e.tile.IsUnitSize());
		toolButtons[ToolType.RECT_FILL].IsShown = (e.tile.IsUnitSize());
	}

	private void ToolChanged(object sender, ToolChangedEventArgs e)
	{

	}

	private void UndoRedo(object sender, UndoRedoEventArgs e)
	{
		toolButtons[ToolType.UNDO].IsShown = (e.undoSize > 0);
		toolButtons[ToolType.REDO].IsShown = (e.redoSize > 0);
	}
}
