/*	Controls the placement of tiles into the world and the undo/redo function.
 *	Also controls grabbing tiles and moving them around.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class CreatorPlayer : MonoBehaviour 
{
	// The tile placement raycast only looks at certain layers.
	protected LayerMask mask;

	// Root transform for all spawned tiles.
	protected Transform spawnRoot;

	// A preview version of the block you're holding.
	protected CreatorTile previewBlock;

	// Currently selected tile and tool.
	protected TileData activeTile;
	protected ToolType activeTool = ToolType.PENCIL;

	// The undo/redo system relies on stacks.
	protected Stack<HashSet<TileOperation>> undoStack;
	protected Stack<HashSet<TileOperation>> redoStack;

	// Variables for drawing routines.
    protected Coroutine drawingRoutine;
	protected bool stopDrawing = false;

	// A tile selected using the Grab tool. Null if no tile is selected.
	protected CreatorTile grabbedTile;

	// Reference to the wrapper object.
	protected CreatorPlayerWrapper wrapper;
	protected int id;

	// Fire an event when the undo or redo stack is modified.
	public event UndoRedoEventHandler UndoRedo;

	protected virtual void OnUndoRedo(UndoRedoEventArgs e)
	{
		UndoRedoEventHandler handler = UndoRedo;

		if(handler != null)
			handler(this, e);
	}

	public delegate void UndoRedoEventHandler(object sender, 
		UndoRedoEventArgs e);

	// Fire an event when the desktop tile changes.
	public event TileChangedEventHandler TileChanged;

	protected virtual void OnTileChanged(TileChangedEventArgs e)
	{
		TileChangedEventHandler handler = TileChanged;

		if(handler != null)
			handler(this, e);
	}

	public delegate void TileChangedEventHandler(object sender, 
		TileChangedEventArgs e);

	// Fire an event when the desktop tool changes.
	public event ToolChangedEventHandler ToolChanged;

	protected virtual void OnToolChanged(ToolChangedEventArgs e)
	{
		ToolChangedEventHandler handler = ToolChanged;

		if(handler != null)
			handler(this, e);
	}

	public delegate void ToolChangedEventHandler(object sender, 
		ToolChangedEventArgs e);

	protected virtual void Start()
	{
		undoStack = new Stack<HashSet<TileOperation>>();
		redoStack = new Stack<HashSet<TileOperation>>();

		OnUndoRedo(new UndoRedoEventArgs(undoStack.Count, redoStack.Count));
	}

	public void SetParameters(CreatorPlayerWrapper wrapper, int id, 
		Transform spawnRoot, LayerMask mask)
	{
		this.wrapper = wrapper;
		this.id = id;
		this.spawnRoot = spawnRoot;
		this.mask = mask;
	}

	// Add a set of operations to the undo history and erase the redo stack.
	protected void AddUndoHistory(HashSet<TileOperation> operations)
	{
		undoStack.Push(operations);
		redoStack.Clear();

		OnUndoRedo(new UndoRedoEventArgs(undoStack.Count, redoStack.Count));
	}

	// Revert the state of the level back to before an operation.
	public void Undo()
	{
		if (undoStack.Count > 0)
		{
			HashSet<TileOperation> ops = undoStack.Pop();

			foreach (TileOperation op in ops)
				op.Undo();

			redoStack.Push(ops);

			OnUndoRedo(new UndoRedoEventArgs(undoStack.Count, redoStack.Count));
		}
	}

	// Reapply the last change that was undone.
	public void Redo()
	{
		if (redoStack.Count > 0)
		{
			HashSet<TileOperation> ops = redoStack.Pop();

			foreach (TileOperation op in ops)
				op.Redo();

			undoStack.Push(ops);

			OnUndoRedo(new UndoRedoEventArgs(undoStack.Count, redoStack.Count));
		}
	}

	// Remove the entire history stacks.
	public void DeleteUndoHistory()
	{
		undoStack = new Stack<HashSet<TileOperation>>();
		redoStack = new Stack<HashSet<TileOperation>>();
	}

	// Change the active tool.
	public void SetActiveTool(ToolType tool)
	{
		activeTool = tool;

		if (previewBlock != null)
			Destroy(previewBlock.gameObject);

		if (activeTool != ToolType.ERASER)
			CreatePreview();

		//OnToolChanged(new ToolChangedEventArgs(tool));
	}

	// Set the tile to be drawn.
	public void SetActiveTile(TileData tile)
	{
		if(tile.name != activeTile.name)
		{
			activeTile = tile;

			if (previewBlock != null)
				Destroy(previewBlock.gameObject);

			if (activeTool == ToolType.PENCIL)
				CreatePreview();

			OnTileChanged(new TileChangedEventArgs(tile));
		}
	}

    private void CreatePreview()
	{
        if (previewBlock != null)
            Destroy(previewBlock.gameObject);

        previewBlock = Instantiate(activeTile.creatorPrefab);
		Destroy(previewBlock.GetComponent<Rigidbody2D>());
	}

	protected abstract void UpdatePreviewPos();

	// Start a drawing operation (pencil, eraser, rectangle etc).
	protected void StartDraw()
	{
		if (drawingRoutine != null)
			StopDraw();

		switch (activeTool)
		{
			case ToolType.PENCIL:
				drawingRoutine = StartCoroutine(PencilDraw());
				break;
			case ToolType.ERASER:
				drawingRoutine = StartCoroutine(Erase());
				break;
			case ToolType.RECT_HOLLOW:
				drawingRoutine = StartCoroutine(DrawRect(false));
				break;
			case ToolType.RECT_FILL:
				drawingRoutine = StartCoroutine(DrawRect(true));
				break;
		}
	}

	// Tell the running coroutine to stop drawing.
	public void StopDraw()
	{
		stopDrawing = true;
	}

	protected abstract IEnumerator PencilDraw();

	// Places a tile at a position where possible.
	protected HashSet<TileOperation> TryPlaceTile(
		HashSet<TileOperation> operations, CreatorTile tile, Vector2 pos)
	{
		RaycastHit2D hitObj = Physics2D.Raycast(pos, Vector3.up, 0.25f, mask);
		CreatorTile existingTile = null;

		if(hitObj.transform != null)
			existingTile = hitObj.transform.GetComponent<CreatorTile>();

		// Don't place tiles if the result would be the same.
		bool sameTile = (existingTile != null && existingTile.GetTilePrefab() == tile);
		// Don't replace air with air.
		bool bothAir = (existingTile == null && tile == null);

		if(!sameTile && !bothAir)
			operations.Add(new TileOperation(tile, existingTile, pos));

		return operations;
	}

	protected abstract IEnumerator Erase();

	protected abstract void FloodFill();

	protected abstract IEnumerator DrawRect(bool filled);

	protected HashSet<CreatorTile> RectHelper(Vector2 startPos, Vector2 endPos, 
		bool filled, bool preview)
	{
		HashSet<CreatorTile> tiles = new HashSet<CreatorTile>();
		HashSet<TileOperation> operations = new HashSet<TileOperation>();

		int minX = (int)Mathf.Min(startPos.x, endPos.x);
		int minY = (int)Mathf.Min(startPos.y, endPos.y);
		int maxX = (int)Mathf.Max(startPos.x, endPos.x);
		int maxY = (int)Mathf.Max(startPos.y, endPos.y);

		for(int x = minX; x <= maxX; ++x)
		{
			for(int y = minY; y <= maxY; ++y)
			{
				// If a position is at the rect boundary, draw the preview here.
				if(filled || x == minX || x == maxX || y == minY || y == maxY)
				{
					if(preview)
					{
						CreatorTile newTile = 
							Instantiate(activeTile.creatorPrefab, 
								new Vector2(x, y), Quaternion.identity);

						Destroy(newTile.GetComponent<Rigidbody2D>());

						tiles.Add(newTile);
					}
					else
					{
						operations = TryPlaceTile(operations, 
							activeTile.creatorPrefab, new Vector2(x, y));
					}
				}
			}
		}

		// Add the drawn tiles to the undo history.
		if(!preview && operations.Count > 0)
			AddUndoHistory(operations);

		return tiles;
	}

	public abstract void ClearAll();

	private void SetPreviewPosition(Vector3 pos)
	{
		if (previewBlock != null)
			previewBlock.transform.position = pos;
	}

	[System.Serializable]
	protected struct TileOperation
	{
		// An instance of the added tile.
		public CreatorTile newTileInst;

		// The prefab of the added tile.
		public CreatorTile newTilePre;

		// The instance of the replaced tile.
		public CreatorTile oldTileInst;

		// The prefab of the replaced tile.
		public CreatorTile oldTilePre;

		public bool oldState;

		// Transform properties.
		public Vector3 position;

		// Create a new block and remove an old one.
		public TileOperation(CreatorTile newTilePre, CreatorTile oldTileInst, 
			Vector3 position)
		{
			this.newTilePre = newTilePre;
			this.position = position;
			this.oldTileInst = oldTileInst;

			oldState = (oldTileInst != null) ? 
				oldTileInst.gameObject.activeSelf : false;

			oldTilePre = null;
			newTileInst = null;

			if (oldTileInst != null)
			{
				oldTilePre = oldTileInst.GetTilePrefab();
				oldTileInst.gameObject.SetActive(false);
			}

			if (newTilePre != null)
			{
				newTileInst = Instantiate(newTilePre, position, 
					Quaternion.identity, LevelEditor.editor.tileRoot);
				newTileInst.SetTilePrefab(newTilePre);
				CreatorPlayerWrapper.Get().AddTile(newTileInst);
			}
		}

		// Replace the new block with the old one.
		public void Undo()
		{
			if (newTileInst != null)
				newTileInst.gameObject.SetActive(false);

			if (oldTilePre != null)
				oldTileInst.gameObject.SetActive(oldState);
		}

		// Replace the old block with the new one.
		public void Redo()
		{
			if (oldTileInst != null)
				oldTileInst.gameObject.SetActive(false);

			if (newTileInst != null)
				newTileInst.gameObject.SetActive(true);
		}
	}
}

// Denotes an integer position on the tile grid.
public struct TilePosition
{
	public int x;
	public int y;

	public TilePosition(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public static bool operator ==(TilePosition a, TilePosition b)
	{
		return (a.x == b.x) && (a.y == b.y);
	}

	public static bool operator !=(TilePosition a, TilePosition b)
	{
		return !(a == b);
	}
}

// Denotes a tile position and the tile held at that position.
public struct TileContents
{
	public TilePosition pos;
	public CreatorTile block;

	public TileContents(TilePosition pos, CreatorTile block)
	{
		this.pos = pos;
		this.block = block;
	}
}

public class TileChangedEventArgs : EventArgs
{
	public TileData tile { get; private set; }

	public TileChangedEventArgs(TileData tile)
	{
		this.tile = tile;
	}
}

public class ToolChangedEventArgs : EventArgs
{
	public ToolType tool { get; private set; }

	public ToolChangedEventArgs(ToolType tool)
	{
		this.tool = tool;
	}
}

public class UndoRedoEventArgs : EventArgs
{
	public int undoSize { get; private set; }
	public int redoSize { get; private set; }

	public UndoRedoEventArgs(int undoSize, int redoSize)
	{
		this.undoSize = undoSize;
		this.redoSize = redoSize;
	}
}
