using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MobileEditorPlayer : EditorPlayer 
{
	public MobileConnection conn { private get; set; }

	private bool keepDrawing = true;

	protected override bool CanDrawOverSpace()
	{
		return true;
	}

	protected override GridPosition GetGridPosition()
	{
		Vector3 pos = conn.PointerToWorldPos();
		return new GridPosition((int)pos.x, (int)pos.y);
	}

	public void ChangeTool(ToolType toolType)
	{
		switch(toolType)
		{
			case ToolType.PENCIL:
				drawState = DrawState.PENCIL_IDLE;
				break;
			case ToolType.ERASER:
				drawState = DrawState.ERASER_IDLE;
				break;
			case ToolType.GRAB:
				drawState = DrawState.GRAB_IDLE;
				break;
			case ToolType.RECT_HOLLOW:
				drawState = DrawState.RECT_HOLLOW_IDLE;
				break;
			case ToolType.RECT_FILL:
				drawState = DrawState.RECT_FILL_IDLE;
				break;
		}
	}

	public void ChangeTile(TileType tileType)
	{
		activeTile = tileType;
	}

	public void Undo()
	{
		LevelEditor.instance.editorGrid.UndoPressed(this, null);
	}

	public void Redo()
	{
		LevelEditor.instance.editorGrid.RedoPressed(this, null);
	}

	private void Update()
	{
		if(playerActive)
		{
			GridPosition pos = GetGridPosition();

			switch(drawState)
			{
				case DrawState.PENCIL_IDLE:
					if(Input.GetMouseButtonDown(0))
						StartCoroutine(Draw(DrawState.PENCIL_DRAW,
							DrawState.PENCIL_IDLE, activeTile));
					break;
				case DrawState.ERASER_IDLE:
					if(Input.GetMouseButtonDown(0))
						StartCoroutine(Draw(DrawState.ERASER_DRAW,
							DrawState.ERASER_IDLE, TileType.NONE));
					break;
				case DrawState.RECT_HOLLOW_IDLE:
					if(Input.GetMouseButtonDown(0))
						StartCoroutine(DrawRect(DrawState.RECT_HOLLOW_DRAW,
							DrawState.RECT_HOLLOW_IDLE, false));
					break;
				case DrawState.RECT_FILL_IDLE:
					if(Input.GetMouseButtonDown(0))
						StartCoroutine(DrawRect(DrawState.RECT_FILL_DRAW,
							DrawState.RECT_FILL_IDLE, true));
					break;
			}
		}
	}

	public void StartAction()
	{
		keepDrawing = true;

		switch(drawState)
		{
			case DrawState.PENCIL_IDLE:
				StartCoroutine(Draw(DrawState.PENCIL_DRAW,
					DrawState.PENCIL_IDLE, activeTile));
				break;
			case DrawState.ERASER_IDLE:
				StartCoroutine(Draw(DrawState.ERASER_DRAW,
					DrawState.ERASER_IDLE, TileType.NONE));
				break;
			case DrawState.RECT_HOLLOW_IDLE:
				StartCoroutine(DrawRect(DrawState.RECT_HOLLOW_DRAW,
					DrawState.RECT_HOLLOW_IDLE, false));
				break;
			case DrawState.RECT_FILL_IDLE:
				StartCoroutine(DrawRect(DrawState.RECT_FILL_DRAW,
					DrawState.RECT_FILL_IDLE, true));
				break;
		}
	}

	public void StopAction()
	{
		keepDrawing = false;
	}

	protected override bool ShouldKeepDrawing()
	{
		return keepDrawing;
	}
}
