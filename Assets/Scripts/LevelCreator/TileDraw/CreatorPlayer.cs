/*	Controls the placement of tiles into the world and the undo/redo function.
 */

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
	protected Stack<List<TileOperation>> undoStack;
	protected Stack<List<TileOperation>> redoStack;

    protected Coroutine drawingRoutine;
	protected bool stopDrawing = false;

	protected CreatorPlayerWrapper wrapper;
	protected int id;

	protected virtual void Start()
	{
		undoStack = new Stack<List<TileOperation>>();
		redoStack = new Stack<List<TileOperation>>();

		CheckHistory();
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
	protected void AddUndoHistory(List<TileOperation> operations)
	{
		undoStack.Push(operations);
		redoStack.Clear();

		CheckHistory();
	}

	// Revert the state of the level back to before an operation.
	public void Undo()
	{
		if (undoStack.Count > 0)
		{
			List<TileOperation> ops = undoStack.Pop();

			foreach (TileOperation op in ops)
				op.Undo();

			redoStack.Push(ops);

			CheckHistory();
		}
	}

	// Reapply the last change that was undone.
	public void Redo()
	{
		if (redoStack.Count > 0)
		{
			List<TileOperation> ops = redoStack.Pop();

			foreach (TileOperation op in ops)
				op.Redo();

			undoStack.Push(ops);

			CheckHistory();
		}
	}

	public abstract void CheckHistory();

	// Remove the entire history stacks.
	public void DeleteUndoHistory()
	{
		undoStack = new Stack<List<TileOperation>>();
		redoStack = new Stack<List<TileOperation>>();
	}

	// Change the active tool.
	public void SetActiveTool(ToolType tool)
	{
		activeTool = tool;

		if (previewBlock != null)
			Destroy(previewBlock.gameObject);

		if (activeTool != ToolType.ERASER)
			CreatePreview();
	}

	// Set the tile to be drawn.
	public virtual void SetActiveTile(TileData tile)
	{
		activeTile = tile;

		// Switch from eraser to pencil.
		if(activeTool == ToolType.ERASER)
			SetActiveTool(ToolType.PENCIL);

		if (previewBlock != null)
			Destroy(previewBlock.gameObject);

		if (activeTool == ToolType.PENCIL)
			CreatePreview();
	}

    private void CreatePreview()
	{
        if (previewBlock != null)
            Destroy(previewBlock.gameObject);

        previewBlock = Instantiate(activeTile.creatorPrefab);
		Destroy(previewBlock.GetComponent<Rigidbody2D>());
	}

	protected virtual void Update()
	{
		UpdatePreviewPos();
	}

	protected abstract void UpdatePreviewPos();

	protected void StartDraw()
	{
		if (drawingRoutine != null)
			StopDraw();

		switch (activeTool)
		{
			case ToolType.PENCIL:
				drawingRoutine = StartCoroutine(PencilDraw(activeTile.creatorPrefab));
				break;
			case ToolType.ERASER:
				drawingRoutine = StartCoroutine(Erase());
				break;
		}
	}

	public void StopDraw()
	{
		// Don't just stop it, because this might break things.
		//StopCoroutine(drawingRoutine);

		stopDrawing = true;
	}

	protected abstract IEnumerator PencilDraw(CreatorTile newTilePre);

	protected abstract IEnumerator Erase();

	protected abstract void FloodFill();

	protected abstract void DrawHollowRect();

	protected abstract void DrawFullRect();

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

		// Transform properties.
		public Vector3 position;

		// Create a new block and remove an old one.
		public TileOperation(CreatorTile newTilePre, CreatorTile oldTileInst, Vector3 position)
		{
			this.newTilePre = newTilePre;
			this.position = position;
			this.oldTileInst = oldTileInst;

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
					Quaternion.identity, CreatorPlayerWrapper.Get().GetRoot());
				newTileInst.SetTilePrefab(newTilePre);
			}
		}

		// Replace the new block with the old one.
		public void Undo()
		{
			if (newTileInst != null)
				newTileInst.gameObject.SetActive(false);

			if (oldTilePre != null)
				oldTileInst.gameObject.SetActive(true);
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

public enum ToolType
{
	PENCIL, ERASER, GRAB, FILL, RECT_HOLLOW, RECT_FILL
}
