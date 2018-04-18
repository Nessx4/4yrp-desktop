/*	The EditorGrid is a single-instance class that controls the Model portion
 *	of an MVC system comprised of: 
 *
 *		the EditorPlayer class (and subclasses) - the Controller; 
 *
 *		the set of GameObjects, specifically EditorTiles, that are visible 
 *		on-screen - the View; 
 *
 *		and this class - an abstract data structure denoting the tiles at each
 *		grid space - the Model.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EditorGrid : MonoBehaviour
{
	private GridTile[,] gridTiles;

	private Stack<List<GridOperation>> undoStack;
	private Stack<List<GridOperation>> redoStack;

	private List<GridOperation> localChanges;

	private void Awake()
	{
		// Register self on the LevelEditor service locator.
		LevelEditor.instance.editorGrid = this;

		gridTiles = new GridTile[100, 100];
	}

	private void Start()
	{
		LevelEditor.instance.toolbar.UndoPressed  += UndoPressed;
		LevelEditor.instance.toolbar.RedoPressed  += RedoPressed;
		LevelEditor.instance.toolbar.ClearPressed += ClearPressed;
		LevelEditor.instance.themebar.ThemeChanged += ThemeChanged;

		undoStack = new Stack<List<GridOperation>>();
		redoStack = new Stack<List<GridOperation>>();
		localChanges = new List<GridOperation>();
	}

	private void UndoPressed(object sender, EventArgs e)
	{
		if(undoStack.Count > 0)
		{
			List<GridOperation> operations = undoStack.Pop();
			List<GridOperation> redoOperations = new List<GridOperation>();

			foreach(GridOperation operation in operations)
			{
				// Reverse the operation.
				UpdateSpace(operation.position, operation.typeBefore);

				// Add the operation to the redo-stack.
				redoOperations.Add(operation);
			}

			redoStack.Push(redoOperations);
		}
	}

	private void RedoPressed(object sender, EventArgs e)
	{
		if(redoStack.Count > 0)
		{
			List<GridOperation> operations = redoStack.Pop();
			List<GridOperation> undoOperations = new List<GridOperation>();

			foreach(GridOperation operation in operations)
			{
				// Re-do the operation.
				UpdateSpace(operation.position, operation.typeAfter);

				// Add the operation to the undo-stack.
				undoOperations.Add(operation);
			}

			undoStack.Push(undoOperations);
		}
	}

	// Remove all tiles on the interface.
	private void ClearPressed(object sender, EventArgs e)
	{
		for(int i = 0; i < gridTiles.GetLength(0); ++i)
		{
			for(int j = 0; j < gridTiles.GetLength(1); ++j)
			{
				if(gridTiles[i, j] != null)
				{
					localChanges.Add(new GridOperation(new GridPosition(i, j),
						gridTiles[i, j].tileType, TileType.NONE));
					gridTiles[i, j].Kill();
				}

				gridTiles[i, j] = null;
			}
		}

		CommitChanges();
	}

	// Update the contents of every tile with the correctly-themed GridTile.
	private void ThemeChanged(object sender, ThemeChangedEventArgs e)
	{
		for(int i = 0; i < gridTiles.GetLength(0); ++i)
		{
			for(int j = 0; j < gridTiles.GetLength(1); ++j)
			{
				if(gridTiles[i, j] != null)
				{
					var oldObj = gridTiles[i, j];

					var newTile = Instantiate<GridTile>(
						LevelEditor.instance.GetTilePrefab(oldObj.tileType), 
						new Vector3(i, j, 0.0f), Quaternion.identity, 
						transform);

					gridTiles[i, j] = newTile;
					Destroy(oldObj.gameObject);
				}

				gridTiles[i, j] = null;
			}
		}
	}

	// Replace the tile at (x, y) with a different type if applicable.
	public void UpdateSpace(GridPosition pos, TileType tileType)
	{
		if(pos.x >= 0 && pos.x < 100 && pos.y >= 0 && pos.y < 100)
		{
			TileType oldType = TileType.NONE;
			if(gridTiles[pos.x, pos.y] != null)
			{
				oldType = gridTiles[pos.x, pos.y].tileType;
				Destroy(gridTiles[pos.x, pos.y].gameObject);
				gridTiles[pos.x, pos.y] = null;
			}

			if(tileType == TileType.NONE)
				return;

			var newTile = Instantiate<GridTile>(LevelEditor.instance.GetTilePrefab(tileType), 
				new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity, transform);
			newTile.tileType = tileType;

			gridTiles[pos.x, pos.y] = newTile;

			localChanges.Add(new GridOperation(pos, oldType, tileType));
		}
	}

	public void CommitChanges()
	{
		undoStack.Push(localChanges);
		localChanges = new List<GridOperation>();
		redoStack = new Stack<List<GridOperation>>();
	}
}

public struct GridPosition
{
	public readonly int x;
	public readonly int y;

	public GridPosition(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString()
	{
		return "(" + x + ", " + y + ")";
	}
}

public struct GridOperation
{
	public readonly GridPosition position;
	public readonly TileType typeBefore;
	public readonly TileType typeAfter;

	public GridOperation(GridPosition position, TileType typeBefore,
		TileType typeAfter)
	{
		this.position   = position;
		this.typeBefore = typeBefore;
		this.typeAfter  = typeAfter;
	}
}
