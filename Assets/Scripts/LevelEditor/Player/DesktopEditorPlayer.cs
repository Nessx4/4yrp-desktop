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
		Debug.Log("Tool changed: " + e.toolType);
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
		GridPosition pos = MouseToGridPosition();

		if(!EventSystem.current.IsPointerOverGameObject())
		{
			if(drawState == DrawState.PENCIL_IDLE)
			{
				if(Input.GetMouseButtonDown(0))
					StartCoroutine(Draw());
			}
		}
	}

	protected override IEnumerator Draw()
	{
		drawState = DrawState.PENCIL_DRAW;
		var positions = new HashSet<GridPosition>();

		EditorGrid editorGrid = LevelEditor.instance.editorGrid;

		while(Input.GetMouseButton(0))
		{
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				GridPosition pos = MouseToGridPosition();

				if(!positions.Contains(pos))
				{
					positions.Add(pos);
					editorGrid.UpdateSpace(pos, activeTile);
				}
			}

			yield return null;
		}

		drawState = DrawState.PENCIL_IDLE;
	}

	protected override IEnumerator Erase()
	{
		drawState = DrawState.ERASER_DRAW;

		yield return null;

		drawState = DrawState.ERASER_IDLE;
	}

	protected override IEnumerator Grab()
	{
		yield return null;
	}

	private GridPosition MouseToGridPosition()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0.0f;
		mousePos = LevelEditor.instance.mainCamera.ScreenToWorldPoint(mousePos);

		return new GridPosition((int)mousePos.x, (int)mousePos.y);
	}
}
