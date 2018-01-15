using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileDrawDesktop : TileDraw 
{
	// Buttons can be greyed out due to tool selection or undo/redo stack size.
	[SerializeField]
	private ToolbarButton undoButton;

	[SerializeField]
	private ToolbarButton redoButton;

	[SerializeField]
	private ToolbarButton fillButton;

	[SerializeField]
	private ToolbarButton rectFilledButton;

	[SerializeField]
	private ToolbarButton rectHollowButton;

	private Camera mainCam;

	private override void Start()
	{
		base.Start();
	}

	// Check if the Undo and Redo buttons need to be greyed out.
	public override void CheckHistory()
	{
		if (undoStack.Count > 0)
			undoButton.Show();
		else
			undoButton.Hide();

		if (redoStack.Count > 0)
			redoButton.Show();
		else
			redoButton.Hide();
	}
}
