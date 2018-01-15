/*	Controls the placement of tiles into the world and the undo/redo function.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TileDraw : MonoBehaviour 
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

	protected TileDrawWrapper wrapper;
	protected int id;

	protected void Start()
	{
		// Camera.main is slow so cache it.
		mainCam = Camera.main;

		undoStack = new Stack<List<TileOperation>>();
		redoStack = new Stack<List<TileOperation>>();

		CheckHistory();
	}

	public void SetParameters(TileDrawWrapper wrapper, int id, 
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
			previewBlock = Instantiate(activeTile.creatorPrefab);
	}

	// Set the tile to be drawn.
	public bool SetActiveTile(TileData tile)
	{
		activeTile = tile;

		// Switch from eraser to pencil.
		if(activeTool == ToolType.ERASER)
			SetActiveTool(ToolType.PENCIL);

		if (previewBlock != null)
			Destroy(previewBlock.gameObject);

		if (activeTool == ToolType.PENCIL)
			CreatePreview();

		return tile.IsUnitSize();
	}

    private void CreatePreview()
	{
        if (previewBlock != null)
            Destroy(previewBlock.gameObject);

        previewBlock = Instantiate(activeTile.creatorPrefab);
		Destroy(previewBlock.GetComponent<Rigidbody2D>());
		Destroy(previewBlock.GetComponent<CreatorTile>());
	}

	public Transform GetRoot()
	{
		return spawnParent;
	}

	// Immediately set the position of the cursor.
	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}

	public void Move(Vector2 moveAmount)
	{
		transform.Translate(moveAmount);
	}

	public void StartDraw()
	{
		if (drawingRoutine != null)
			StopDraw();

		switch (activeTool)
		{
			case ToolType.PENCIL:
				drawingRoutine = StartCoroutine(DrawTiles(activeTile.creatorPrefab));
				break;
			case ToolType.ERASER:
				drawingRoutine = StartCoroutine(EraseTiles());
				break;
		}
	}

	public void StopDraw()
	{
		// Don't just stop it, because this might break things.
		//StopCoroutine(drawingRoutine);

		stopDrawing = true;
	}

	private IEnumerator DrawTiles(CreatorTile newTilePre)
	{
		yield return null;
	}

	private IEnumerator EraseTiles()
	{
		yield return null;
	}

	private void SetPreviewPosition(Vector3 pos)
	{
		if (previewBlock != null)
			previewBlock.transform.position = pos;
	}

	// While still holding down the placement button, continually place or
	// remove tiles.
	private IEnumerator PlaceTiles(CreatorTile newTilePre, int mouseButton)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		List<TilePosition> tilePositions = new List<TilePosition>();

		List<TileOperation> operations = new List<TileOperation>();

		while(!stopDrawing)
		{
			// Do not place tiles when mouse is on top of the UI elements.
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				Vector3 mousePos = new Vector3(Input.mousePosition.x, 
					Input.mousePosition.y, 10.0f);
				Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);

				TilePosition tp = new TilePosition(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

				bool hasPlacedHere = false;

				foreach (TilePosition tilePosition in tilePositions)
				{
					if (tilePosition == tp)
					{
						hasPlacedHere = true;
						continue;
					}
				}

				if(!hasPlacedHere)
				{
					pos.x = tp.x;
					pos.y = tp.y;
					pos.z = 0.0f;

					tilePositions.Add(tp);

					RaycastHit2D hitObj = Physics2D.Raycast(pos, Vector3.up, 0.25f, mask);
					CreatorTile existingTile = null;

					if(hitObj.transform != null)
						existingTile = hitObj.transform.GetComponent<CreatorTile>();

					// Don't place tiles if the result would be the same.
					bool sameTile = (existingTile != null && existingTile.GetTilePrefab() == newTilePre);
					// Don't replace air with air.
					bool bothAir = (existingTile == null && newTilePre == null);

					if(!sameTile && !bothAir)
						operations.Add(new TileOperation(newTilePre, existingTile, pos));
				}
			}

			yield return wait;
		}

		// Add the drawn tiles to the undo history.
		if (operations.Count > 0)
			AddOperations(operations);

		stopDrawing = false;
	}

    // While still holding down the placement button, continually place or
    // remove tiles.
    private IEnumerator PlaceMobileTiles(CreatorTile newTilePre)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        List<TilePosition> tilePositions = new List<TilePosition>();

        List<TileOperation> operations = new List<TileOperation>();

        while (!stopDrawing)
        {
            // Do not place tiles when mouse is on top of the UI elements.
            //if (!EventSystem.current.IsPointerOverGameObject())
            //{
                //Vector3 mousePos = new Vector3(Input.mousePosition.x,
                    //Input.mousePosition.y, 10.0f);
                Vector3 pos = PointerController.control.GetPointerPos(0);

                TilePosition tp = new TilePosition(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

                bool hasPlacedHere = false;

                foreach (TilePosition tilePosition in tilePositions)
                {
                    if (tilePosition == tp)
                    {
                        hasPlacedHere = true;
                        continue;
                    }
                }

                if (!hasPlacedHere)
                {
                    pos.x = tp.x;
                    pos.y = tp.y;
                    pos.z = 0.0f;

                    tilePositions.Add(tp);

                    RaycastHit2D hitObj = Physics2D.Raycast(pos, Vector3.up, 0.25f, mask);
                    CreatorTile existingTile = null;

                    if (hitObj.transform != null)
                        existingTile = hitObj.transform.GetComponent<CreatorTile>();

                    // Don't place tiles if the result would be the same.
                    bool sameTile = (existingTile != null && existingTile.GetTilePrefab() == newTilePre);
                    // Don't replace air with air.
                    bool bothAir = (existingTile == null && newTilePre == null);

                    if (!sameTile && !bothAir)
                    {
                        if(newTilePre != null)
							operations.Add(new TileOperation(newTilePre, existingTile, pos));
				}
             
                }
            //}

            yield return wait;
        }

		// Add the drawn tiles to the undo history.
		if (operations.Count > 0)
			AddOperations(operations);
	}


    // Add a Block to the list of blocks being tracked.
    public void AddBlock(CreatorTile block)
	{
		blocks.Add(block);
	}

	// Get the whole list of Block objects in the level.
	public List<CreatorTile> GetBlocks()
	{
		return blocks;
	}

	public void Clear()
	{
		List<TileOperation> operations = new List<TileOperation>();

		foreach (CreatorTile block in blocks)
			operations.Add(new TileOperation(null, block, block.transform.position));

		AddOperations(operations);
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
				newTileInst = Instantiate(newTilePre, position, Quaternion.identity, placement.GetRoot());
				newTileInst.SetTilePrefab(newTilePre);
				placement.AddBlock(newTileInst);
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
