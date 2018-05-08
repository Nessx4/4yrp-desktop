using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class EditorPlayer : MonoBehaviour 
{
	protected TileType activeTile;

	protected DrawState drawState;

	protected IEnumerator Draw(DrawState beginState, 
		DrawState endState, TileType tileType)
	{
		drawState = beginState;
		var drawnPositions = new HashSet<GridPosition>();

		EditorGrid editorGrid = LevelEditor.instance.editorGrid;

		GridPosition lastPosition = GetGridPosition();

		while(Input.GetMouseButton(0))
		{
			if(CanDrawOverSpace())
			{
				GridPosition endPosition = GetGridPosition();

				var tryPositions = PlotLine(lastPosition, endPosition);

				Vector2 tileSize = LevelEditor.instance.GetTileSize(tileType);

				foreach(GridPosition tryPosition in tryPositions)
				{
					if(!IsOverlap(tryPosition, drawnPositions, tileType))
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

	protected bool IsOverlap(GridPosition tryPosition, 
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

	protected IEnumerator DrawRect(DrawState startState, 
		DrawState endState, bool filled)
	{
		drawState = startState;

		EditorGrid editorGrid = LevelEditor.instance.editorGrid;

		GridPosition startPosition = GetGridPosition();
		GridPosition endPosition = GetGridPosition();

		while(Input.GetMouseButton(0))
		{
			if(CanDrawOverSpace())
			{
				endPosition = GetGridPosition();

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

	// Use Bresenham's Line Algorithm to draw a line between two points.
	protected List<GridPosition> PlotLine(GridPosition a, GridPosition b)
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

	// Auxiliary method for Bresenham's.
	protected List<GridPosition> PlotLineLow(GridPosition a, GridPosition b,
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

	// Auxiliary method for Bresenham's.
	protected List<GridPosition> PlotLineHigh(GridPosition a, GridPosition b,
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

	//protected abstract IEnumerator Erase();
	//protected abstract IEnumerator Grab();

	protected abstract bool CanDrawOverSpace();

	protected abstract GridPosition GetGridPosition();
}
