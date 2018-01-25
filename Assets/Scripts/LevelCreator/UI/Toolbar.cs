/*	Toolbar governs the bar at the top of the Level Editor UI that contains
 *	the basic drawing tools, save and undo/redo.
 */
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour 
{
	[SerializeField]
	private ToolbarButton activeTool;

	[SerializeField] 
	private List<ToolbarButton> buttons;
	private Dictionary<ToolType, ToolbarButton> toolButtons = 
		new Dictionary<ToolType, ToolbarButton>();

	private Color activeColor = new Color(0.85f, 0.55f, 0.77f, 1.0f);

	private void Start()
	{
		activeTool.SetColor(activeColor);

		foreach(var button in buttons)
			toolButtons.Add(button.Tool, button);

		// By default, neither undo nor redo buttons are shown.
		toolButtons[ToolType.UNDO].IsShown = false;
		toolButtons[ToolType.REDO].IsShown = false;

		CreatorPlayerWrapper.Get().GetPlayer(0).TileChanged += TileChanged;
		//CreatorPlayerWrapper.Get().GetPlayer(0).ToolChanged += ToolChanged;
		CreatorPlayerWrapper.Get().GetPlayer(0).UndoRedo += UndoRedo;
	}

	// Set a tool directly from a toolbar button.
	public void SetTool(ToolbarButton newTool, bool update)
	{
		activeTool.SetColor(Color.white);
		activeTool = newTool;
		activeTool.SetColor(activeColor);

		if(update)
			CreatorPlayerWrapper.Get().SetActiveTool(0, activeTool.Tool);
	}

	// For certain buttons, only active if the tile is unit size.
	private void TileChanged(object sender, TileChangedEventArgs e)
	{
		toolButtons[ToolType.FILL].IsShown = (e.tile.IsUnitSize());
		toolButtons[ToolType.RECT_HOLLOW].IsShown = (e.tile.IsUnitSize());
		toolButtons[ToolType.RECT_FILL].IsShown = (e.tile.IsUnitSize());

		// If our selected tool just became invalid, switch to Pencil tool.
		if(activeTool.Tool == ToolType.ERASER || !activeTool.IsShown)
			SetTool(toolButtons[ToolType.PENCIL], true);
	}

	/*
	private void ToolChanged(object sender, ToolChangedEventArgs e)
	{
		
	}
	*/

	private void UndoRedo(object sender, UndoRedoEventArgs e)
	{
		toolButtons[ToolType.UNDO].IsShown = (e.undoSize > 0);
		toolButtons[ToolType.REDO].IsShown = (e.redoSize > 0);
	}
}
