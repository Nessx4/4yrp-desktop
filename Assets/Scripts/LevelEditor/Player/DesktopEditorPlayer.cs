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
		GridPosition pos = MouseToGridPosition();

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

	protected override IEnumerator Draw(DrawState beginState, 
		DrawState endState, TileType tileType)
	{
		drawState = beginState;
		var drawnPositions = new HashSet<GridPosition>();

		EditorGrid editorGrid = LevelEditor.instance.editorGrid;

		GridPosition lastPosition = MouseToGridPosition();

		while(Input.GetMouseButton(0))
		{
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				GridPosition endPosition = MouseToGridPosition();

				var tryPositions = PlotLine(lastPosition, endPosition);

				Vector2 tileSize = LevelEditor.instance.GetTileSize(tileType);

				foreach(GridPosition tryPosition in tryPositions)
				{
					if(!IsOverlap(tryPosition, drawnPositions, tileType))
					//if(!drawnPositions.Contains(tryPosition))
					{
						// Add all positions taken up by new tile.
						for(int x = tryPosition.x; x < tryPosition.x + tileSize.x; ++x)
						{
							for(int y = tryPosition.y; y < tryPosition.y + tileSize.y; ++y)
								drawnPositions.Add(new GridPosition(x, y));
						}
						editorGrid.PlaceTile(tryPosition, tileType);
					}
				}

				lastPosition = endPosition;
			}

			yield return null;
		}

		drawState = endState;
		editorGrid.CommitChanges();
	}

	private bool IsOverlap(GridPosition tryPosition, 
		HashSet<GridPosition> drawnPositions, TileType tileType)
	{
		Vector2 tileSize = LevelEditor.instance.GetTileSize(tileType);

		for(int x = tryPosition.x; x < tryPosition.x + tileSize.x; ++x)
		{
			for(int y = tryPosition.y; y < tryPosition.y + tileSize.y; ++y)
			{
				if(drawnPositions.Contains(new GridPosition(x, y)))
					return true;
			}
		}

		return false;
	}

	protected override IEnumerator DrawRect(DrawState startState, 
		DrawState endState, bool filled)
	{
		drawState = startState;

		EditorGrid editorGrid = LevelEditor.instance.editorGrid;

		GridPosition startPosition = MouseToGridPosition();
		GridPosition endPosition = MouseToGridPosition();

		while(Input.GetMouseButton(0))
		{
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				endPosition = MouseToGridPosition();

				yield return null;
			}
		}

		int startX = Mathf.Min(startPosition.x, endPosition.x);
		int startY = Mathf.Min(startPosition.y, endPosition.y);
		int endX   = Mathf.Max(startPosition.x, endPosition.x);
		int endY   = Mathf.Max(startPosition.y, endPosition.y);

		for(int x = startX; x <= endX; ++x)
		{
			for(int y = startY; y <= endY; ++y)
			{
				if(y == startY || y == endY || x == startX || x == endX || filled)
					editorGrid.PlaceTile(new GridPosition(x, y), activeTile);
			}
		}

		drawState = endState;
		editorGrid.CommitChanges();
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

	private List<GridPosition> PlotLine(GridPosition a, GridPosition b)
	{
		List<GridPosition> positions = new List<GridPosition>();

		if(Mathf.Abs(b.y - a.y) < Mathf.Abs(b.x - a.x))
		{
			if(a.x > b.x)
				positions = PlotLineLow(b, a, positions);
			else
				positions = PlotLineLow(a, b, positions);
		}
		else
		{
			if(a.y > b.y)
				positions = PlotLineHigh(b, a, positions);
			else
				positions = PlotLineHigh(a, b, positions);
		}

		return positions;
	}

	private List<GridPosition> PlotLineLow(GridPosition a, GridPosition b,
		List<GridPosition> positions)
	{
		int dx = b.x - a.x;
		int dy = b.y - a.y;
		int yi = 1;

		if(dy < 0)
		{
			yi = -1;
			dy = -dy;
		}

		int D = 2 * dy - dx;
		int y = a.y;

		for(int x = a.x; x <= b.x; ++x)
		{
			positions.Add(new GridPosition(x, y));

			if(D > 0)
			{
				y = y + yi;
				D = D - 2 * dx;
			}

			D = D + 2 * dy;
		}

		return positions;
	}

	private List<GridPosition> PlotLineHigh(GridPosition a, GridPosition b,
		List<GridPosition> positions)
	{
		int dx = b.x - a.x;
		int dy = b.y - a.y;
		int xi = 1;

		if(dx < 0)
		{
			xi = -1;
			dx = -dx;
		}

		int D = 2 * dx - dy;
		int x = a.x;

		for(int y = a.y; y <= b.y; ++y)
		{
			positions.Add(new GridPosition(x, y));

			if(D > 0)
			{
				x = x + xi;
				D = D - 2 * dy;
			}

			D = D + 2 * dx;
		}

		return positions;
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
