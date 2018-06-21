using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopEditorPlayer : EditorPlayer 
{
	private void OnEnable()
	{
		// Register events.
		LevelEditor.instance.toolbar.ToolChanged += ToolChanged;
		LevelEditor.instance.palette.TileChanged += TileChanged;
	}

	private void ToolChanged(object sender, ToolChangedEventArgs e)
	{
		switch(e.toolType)
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

	private void TileChanged(object sender, TileChangedEventArgs e)
	{
		activeTile = e.tileType;
	}

	private void Update()
	{
		GridPosition pos = GetGridPosition();

		if(!EventSystem.current.IsPointerOverGameObject())
		{
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

	protected override bool CanDrawOverSpace()
	{
		return !EventSystem.current.IsPointerOverGameObject();
	}

	/*
	protected override IEnumerator Erase()
	{
		drawState = DrawState.ERASER_DRAW;
		var drawnPositions = new HashSet<GridPosition>();

		EditorGrid editorGrid = LevelEditor.instance.editorGrid;

		GridPosition lastPosition = MouseToGridPosition();

		while(Input.GetMouseButton(0))
		{
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				GridPosition endPosition = MouseToGridPosition();

				var tryPositions = PlotLine(lastPosition, endPosition);

				foreach(GridPosition tryPosition in tryPositions)
				{
					if(!drawnPositions.Contains(tryPosition))
					{
						drawnPositions.Add(tryPosition);
						editorGrid.UpdateSpace(tryPosition, TileType.NONE);
					}
				}

				lastPosition = endPosition;
			}

			yield return null;
		}

		drawState = DrawState.ERASER_IDLE;
	}
	*/

	/*
	protected override IEnumerator Grab()
	{
		yield return null;
	}
	*/

	protected override GridPosition GetGridPosition()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0.0f;
		mousePos = LevelEditor.instance.mainCamera.ScreenToWorldPoint(mousePos);

		return new GridPosition((int)mousePos.x, (int)mousePos.y);
	}

	protected override bool ShouldKeepDrawing()
	{
		return Input.GetMouseButton(0);
	}
}
