using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

public class Toolbar : MonoBehaviour
{
	[SerializeField]
	private ToolbarButton[] buttons;

	private ToolType activeTool = ToolType.PENCIL;

	public event EventHandler UndoPressed;
	public event EventHandler RedoPressed;
	public event EventHandler ClearPressed;

	public event EventHandler SavePressed;
	public event EventHandler PlayPressed;

	public event EventHandler MenuPressed;

	public event ToolChangedEventHandler ToolChanged;

	public delegate void ToolChangedEventHandler(object sender,
		ToolChangedEventArgs e);

	// Fire this event whenever the Undo button is pressed.
	protected virtual void OnUndoPressed(EventArgs e)
	{
		EventHandler handler = UndoPressed;
		if(handler != null)
			handler(this, e);
	}

	// Fire this event whenever the Redo button is pressed.
	protected virtual void OnRedoPressed(EventArgs e)
	{
		EventHandler handler = RedoPressed;
		if(handler != null)
			handler(this, e);
	}

	// Fire this event whenever the Clear button is pressed.
	protected virtual void OnClearPressed(EventArgs e)
	{
		EventHandler handler = ClearPressed;
		if(handler != null)
			handler(this, e);
	}

	// Fire this event whenever the Save button is pressed.
	protected virtual void OnSavePressed(EventArgs e)
	{
		EventHandler handler = SavePressed;
		if(handler != null)
			handler(this, e);
	}

	// Fire this event whenever the Play button is pressed.
	protected virtual void OnPlayPressed(EventArgs e)
	{
		EventHandler handler = PlayPressed;
		if(handler != null)
			handler(this, e);
	}

	// Fire this event whenever the Menu button is pressed.
	protected virtual void OnMenuPressed(EventArgs e)
	{
		EventHandler handler = MenuPressed;
		if(handler != null)
			handler(this, e);
	}

	// Fire this event whenever the selected tool changes.
	protected virtual void OnToolChanged(ToolChangedEventArgs e)
	{
		ToolChangedEventHandler handler = ToolChanged;
		if(handler != null)
			handler(this, e);
	}

	private void Awake()
	{
		// Register self on the LevelEditor service locator.
		LevelEditor.instance.toolbar = this;

		// Assert that there are 11 buttons on the Toolbar.
		Assert.AreEqual(buttons.Length, 11, "Incorrect no. of ToolbarButtons.");

		// All ToolbarButtons will be coupled directly to the Toolbar class for
		// ease.
		foreach(var button in buttons)
			button.toolbar = this;
	}

	private void Start()
	{
		// Register an event.
		LevelEditor.instance.palette.TileChanged += TileChanged;
	}

	private void TileChanged(object sender, TileChangedEventArgs e)
	{
		Debug.Log("TileChanged event received.\n" + 
			"You should probably do something about it, hmmmm?");

		//throw new NotImplementedException("Need to deactivate certain tools.");
	}

	public void ChangeTool(ToolType newTool)
	{
		Debug.Log("Do the thing");
		if(newTool != activeTool)
		{
			activeTool = newTool;
			OnToolChanged(new ToolChangedEventArgs(newTool));

			throw new NotImplementedException("Incomplete - must set correct ToolbarButtons active/inactive.");
		}
	}

	public void ActionPressed(ActionType actionType)
	{
		EventArgs e = new EventArgs();
		switch(actionType)
		{
			case ActionType.UNDO:
				OnUndoPressed(e);
				break;
			case ActionType.REDO:
				OnRedoPressed(e);
				break;
			case ActionType.CLEAR:
				OnClearPressed(e);
				break;
			case ActionType.SAVE:
				OnSavePressed(e);
				break;
			case ActionType.PLAY:
				OnPlayPressed(e);
				break;
			case ActionType.MENU:
				OnMenuPressed(e);
				break;
		}
	}
}

public class ToolChangedEventArgs : EventArgs
{
	// Don't want the ToolType to be changed by the event listener.
	public readonly ToolType toolType;

	public ToolChangedEventArgs(ToolType toolType)
	{
		this.toolType = toolType;
	}
}
