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
using UnityEngine.Assertions;

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
				PlaceTile(operation.position, operation.typeBefore);

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
				PlaceTile(operation.position, operation.typeAfter);

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

	public void PlaceTile(GridPosition pos, TileType tileType)
	{
		if(pos.x >= 0 && pos.x < 100 && pos.y >= 0 && pos.y < 100)
		{
			Vector2 newTileSize = LevelEditor.instance.GetTileSize(tileType);
			TileType oldType = TileType.NONE;

			// Remove all old tiles that will be overwritten by the new one.
			for(int x = pos.x; x < pos.x + newTileSize.x; ++x)
			{
				for(int y = pos.y; y < pos.y + newTileSize.y; ++y)
				{
					Vector2 oldTileSize;
					if(gridTiles[x, y] != null)
					{
						GridTile oldTile = gridTiles[x, y];
						oldType = oldTile.tileType;
						GridPosition oldPosition = oldTile.position;

						oldTileSize = LevelEditor.instance.GetTileSize(oldType);

						// Update the Model by removing the old TileType.
						gridTiles[oldPosition.x, oldPosition.y] = null;

						// Destroy the View component of the MVC.
						Destroy(oldTile.gameObject);
					}
				}
			}

			// Don't instantiate new tiles if the type is NONE.
			if(tileType == TileType.NONE)
				return;

			var newTile = Instantiate<GridTile>(LevelEditor.instance.GetTilePrefab(tileType), 
				new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity, transform);
			newTile.tileType = tileType;
			newTile.position = pos;

			// Update the model with the new TileType.
			gridTiles[pos.x, pos.y] = newTile;

			localChanges.Add(new GridOperation(pos, oldType, tileType));
		}
	}

	// Return the types of tile on the Model.
	public TileType[,] GetTileTypes()
	{
		TileType[,] tileTypes = new TileType[100, 100];

		Assert.AreEqual(100, gridTiles.GetLength(0));
		Assert.AreEqual(100, gridTiles.GetLength(1));

		Debug.Log(tileTypes);
		Debug.Log(gridTiles);

		for(int x = 0; x < gridTiles.GetLength(0); ++x)
		{
			for(int y = 0; y < gridTiles.GetLength(1); ++y)
			{
				tileTypes[x, y] = (gridTiles[x, y] == null) ?
					TileType.NONE : gridTiles[x, y].tileType;
			}
		}

		return tileTypes;
	}

	// Clear everything on the Model and View and load a new set of tiles.
	public void SetTileTypes(TileType[,] tileTypes)
	{
		// Don't clear if there was no data.
		if(tileTypes == null)
			return;

		for(int x = 0; x < gridTiles.GetLength(0); ++x)
		{
			for(int y = 0; y < gridTiles.GetLength(1); ++y)
			{
				if(gridTiles[x, y] != null)
					Destroy(gridTiles[x, y].gameObject);

				if(tileTypes[x, y] != TileType.NONE)
					PlaceTile(new GridPosition(x, y), tileTypes[x, y]);
			}
		}
	}

	/*
	// Replace the tile at (x, y) with a different type if applicable.
	public void UpdateSpace(GridPosition pos, TileType tileType)
	{
		if(pos.x >= 0 && pos.x < 100 && pos.y >= 0 && pos.y < 100)
		{
			TileType oldType = TileType.NONE;
			Vector2 tileSize;
			if(gridTiles[pos.x, pos.y] != null)
			{
				GridTile oldTile = gridTiles[pos.x, pos.y];
				oldType = oldTile.tileType;
				GridPosition oldPosition = oldTile.position;

				tileSize = LevelEditor.instance.GetTileSize(oldType);

				// Update the Model by removing the old TileType.
				for(int x = oldPosition.x; x < oldPosition.x + tileSize.x; ++x)
				{
					for(int y = oldPosition.y; y < oldPosition.y + tileSize.y; ++y)
						gridTiles[x, y] = null;
				}
				// Destroy the View component of the MVC.
				Destroy(oldTile.gameObject);
			}

			// Don't instantiate new tiles if the type is NONE.
			if(tileType == TileType.NONE)
				return;

			var newTile = Instantiate<GridTile>(LevelEditor.instance.GetTilePrefab(tileType), 
				new Vector3(pos.x, pos.y, 0.0f), Quaternion.identity, transform);
			newTile.tileType = tileType;
			newTile.position = pos;

			tileSize = LevelEditor.instance.GetTileSize(tileType);

			// Update the model with the new TileType.
			for(int x = pos.x; x < pos.x + tileSize.x; ++x)
			{
				for(int y = pos.y; y < pos.y + tileSize.y; ++y)
					gridTiles[x, y] = newTile;
			}

			localChanges.Add(new GridOperation(pos, oldType, tileType));
		}
	}
	*/

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

	public override bool Equals(object obj)
	{
		if(obj == null || GetType() != obj.GetType())
			return false;

		GridPosition other = (GridPosition)obj;

		return (x == other.x && y == other.y);
	}

	public static bool operator==(GridPosition a, GridPosition b)
	{
		return a.Equals(b);
	}

	public static bool operator!=(GridPosition a, GridPosition b)
	{
		return !a.Equals(b);
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
