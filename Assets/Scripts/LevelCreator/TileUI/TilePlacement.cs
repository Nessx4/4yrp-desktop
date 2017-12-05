/*	Controls the placement of tiles into the world and the undo/redo function.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class TilePlacement : MonoBehaviour 
{
	// The tile placement raycast only looks at certain layers.
	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private Transform tileRoot;
	private List<Block> blocks;

	[SerializeField]
	private Block startTiles;

	[SerializeField]
	private ToolbarButton undoButton;

	[SerializeField]
	private ToolbarButton redoButton;

	[SerializeField]
	private ToolbarButton fillButton;

	[SerializeField]
	private ToolbarButton rectFilledButton;

	[SerializeField]
	private ToolbarButton rectHollowButton;

	// A visible version of the Block you have selected at the mouse position.
	private Block previewBlock;

    private Block previewBlockMob;

	// Currently selected tile.
	private TileData activeTile;

    private TileData activeTileMob;

	// The undo/redo system relies on stacks.
	private Stack<List<TileOperation>> undoStack;
	private Stack<List<TileOperation>> redoStack;

	private Stack<List<TileOperation>> undoStackMob;
	private Stack<List<TileOperation>> redoStackMob;

	private ToolType activeTool = ToolType.PENCIL;

    private ToolType activeToolMob = ToolType.PENCIL;

	private Camera mainCam;

    private Coroutine mobileDraw;
	private bool stopDrawing = false;

    private Vector3 mobilePos;

	public static TilePlacement placement;

	private void Start()
	{
		placement = this;

		// Camera.main is slow so cache it.
		mainCam = Camera.main;

		undoStack = new Stack<List<TileOperation>>();
		redoStack = new Stack<List<TileOperation>>();

		undoStackMob = new Stack<List<TileOperation>>();
		redoStackMob = new Stack<List<TileOperation>>();

		blocks = new List<Block>();

		CheckUndoRedo();
	}

	// Change the active tool.
	public void SetActiveTool(ToolType tool)
	{
		activeTool = tool;

		if (previewBlock != null)
			Destroy(previewBlock.gameObject);

		if (activeTool == ToolType.PENCIL)
			previewBlock = Instantiate(activeTile.tilePrefab);
	}

	public void SetActiveTile(TileData tile)
	{
		activeTile = tile;

		if(tile.IsUnitSize())
		{
			fillButton.Show();
			rectFilledButton.Show();
			rectHollowButton.Show();
		}
		else
		{
			fillButton.Hide();
			rectFilledButton.Hide();
			rectHollowButton.Hide();
		}

		if (previewBlock != null)
			Destroy(previewBlock.gameObject);

		if (activeTool == ToolType.PENCIL)
			CreatePreview();
	}

    // Change the active tool.
    public void SetActiveToolMobile(ToolType tool)
    {
        activeToolMob = tool;

        if (previewBlockMob != null)
            Destroy(previewBlockMob.gameObject);

        if (activeToolMob == ToolType.PENCIL)
            previewBlockMob = Instantiate(activeTileMob.tilePrefab);
    }

    public void SetActiveTileMobile(TileData tile)
    {
        activeTileMob = tile;

        if (previewBlockMob != null)
            Destroy(previewBlock.gameObject);

        if (activeToolMob == ToolType.PENCIL)
            CreatePreviewMobile();
    }

    private void CreatePreview()
	{
		previewBlock = Instantiate(activeTile.tilePrefab);
		Destroy(previewBlock.GetComponent<Rigidbody2D>());
	}

    private void CreatePreviewMobile()
    {
        previewBlockMob = Instantiate(activeTileMob.tilePrefab);
        Destroy(previewBlockMob.GetComponent<Rigidbody2D>());
    }

	public void DeleteUndoHistory()
	{
		undoStack = new Stack<List<TileOperation>>();
		redoStack = new Stack<List<TileOperation>>();
	}

	public Transform GetRoot()
	{
		return tileRoot;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if(activeTool == ToolType.PENCIL)
				StartCoroutine(PlaceTiles(activeTile.tilePrefab, 0));
			else if(activeTool == ToolType.ERASER)
				StartCoroutine(PlaceTiles(null, 0));
		}

		if(previewBlock != null)
		{
			Vector3 mousePos = new Vector3(Input.mousePosition.x, 
				Input.mousePosition.y, 10.0f);
			Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);

			pos.x = Mathf.RoundToInt(pos.x);
			pos.y = Mathf.RoundToInt(pos.y);
			pos.z = -5.0f;

			previewBlock.transform.position = pos;
		}
	}

    public void StartMobileDraw()
    {
        if (mobileDraw != null)
            StopCoroutine(mobileDraw);

		stopDrawing = false;

        if (activeToolMob == ToolType.PENCIL)
            mobileDraw = StartCoroutine(PlaceMobileTiles(activeTileMob.tilePrefab));
        else if (activeToolMob == ToolType.ERASER)
            mobileDraw = StartCoroutine(PlaceMobileTiles(null));

    }

    public void StopMobileDraw()
    {
		stopDrawing = true;
    }

	// While still holding down the placement button, continually place or
	// remove tiles.
	private IEnumerator PlaceTiles(Block newTilePre, int mouseButton)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		List<TilePosition> tilePositions = new List<TilePosition>();

		List<TileOperation> operations = new List<TileOperation>();

		while(Input.GetMouseButton(mouseButton))
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
					Block existingTile = null;

					if(hitObj.transform != null)
						existingTile = hitObj.transform.GetComponent<Block>();

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
		{
			undoStack.Push(operations);
			redoStack.Clear();

			CheckUndoRedo();
		}
	}

    // While still holding down the placement button, continually place or
    // remove tiles.
    private IEnumerator PlaceMobileTiles(Block newTilePre)
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
                    Block existingTile = null;

                    if (hitObj.transform != null)
                        existingTile = hitObj.transform.GetComponent<Block>();

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
		{
			undoStackMob.Push(operations);
			redoStackMob.Clear();

			CheckUndoRedo();
		}
	}


    // Add a Block to the list of blocks being tracked.
    public void AddBlock(Block block)
	{
		blocks.Add(block);
	}

	// Get the whole list of Block objects in the level.
	public List<Block> GetBlocks()
	{
		return blocks;
	}

	// Revert the state of the level back to before an operation.
	public void Undo()
	{
		if (undoStack.Count > 0)
		{
			List<TileOperation> ops = undoStack.Pop();

			foreach(TileOperation op in ops)
				op.Undo();

			redoStack.Push(ops);

			CheckUndoRedo();
		}
	}

	// Reapply the last change that was undone.
	public void Redo()
	{
		if (redoStack.Count > 0)
		{
			List<TileOperation> ops = redoStack.Pop();

			foreach(TileOperation op in ops)
				op.Redo();

			undoStack.Push(ops);

			CheckUndoRedo();
		}
	}

	// Revert the state of the level back to before an operation.
	public void UndoMobile()
	{
		if (undoStackMob.Count > 0)
		{
			List<TileOperation> ops = undoStackMob.Pop();

			foreach (TileOperation op in ops)
				op.Undo();

			redoStackMob.Push(ops);
		}
	}

	// Reapply the last change that was undone.
	public void RedoMobile()
	{
		if (redoStackMob.Count > 0)
		{
			List<TileOperation> ops = redoStackMob.Pop();

			foreach (TileOperation op in ops)
				op.Redo();

			undoStackMob.Push(ops);
		}
	}

	// Check if the Undo and Redo buttons need to be greyed out.
	public void CheckUndoRedo()
	{
		if (undoStack.Count > 0)
			undoButton.Show();
		else
			undoButton.Hide();

		if (redoStack.Count > 0)
			redoButton.Show();
		else
			redoButton.Hide();
	}

	public void Clear()
	{
		List<TileOperation> operations = new List<TileOperation>();

		foreach (Block block in blocks)
			operations.Add(new TileOperation(null, block, block.transform.position));

		undoStack.Push(operations);
		redoStack.Clear();

		CheckUndoRedo();
	}

	[System.Serializable]
	private struct TileOperation
	{
		// An instance of the added tile.
		public Block newTileInst;

		// The prefab of the added tile.
		public Block newTilePre;

		// The instance of the replaced tile.
		public Block oldTileInst;

		// The prefab of the replaced tile.
		public Block oldTilePre;

		// Transform properties.
		public Vector3 position;

		// Create a new block and remove an old one.
		public TileOperation(Block newTilePre, Block oldTileInst, Vector3 position)
		{
			this.newTilePre = newTilePre;
			this.position = position;
			this.oldTileInst = oldTileInst;

			oldTilePre = null;
			newTileInst = null;

			if(oldTileInst != null)
			{
				oldTilePre = oldTileInst.GetTilePrefab();
				oldTileInst.gameObject.SetActive(false);
			}

			if(newTilePre != null)
			{
				newTileInst = Instantiate(newTilePre, position, Quaternion.identity, placement.GetRoot());
				newTileInst.SetTilePrefab(newTilePre);
				placement.AddBlock(newTileInst);
			}
		}

		// Replace the new block with the old one.
		public void Undo()
		{
			if(newTileInst != null)
				newTileInst.gameObject.SetActive(false);

			if(oldTilePre != null)
				oldTileInst.gameObject.SetActive(true);
		}

		// Replace the old block with the new one.
		public void Redo()
		{
			if(oldTileInst != null)
				oldTileInst.gameObject.SetActive(false);

			if(newTileInst != null)
				newTileInst.gameObject.SetActive(true);
		}
	}

	// Denotes an integer position on the tile grid.
	private struct TilePosition
	{
		public int x;
		public int y;

		public TilePosition(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public static bool operator== (TilePosition a, TilePosition b)
		{
			return (a.x == b.x) && (a.y == b.y);
		}

		public static bool operator!= (TilePosition a, TilePosition b)
		{
			return !(a == b);
		}
	}

	// Denotes a tile position and the tile held at that position.
	private struct TileContents
	{
		public TilePosition pos;
		public Block block;

		public TileContents(TilePosition pos, Block block)
		{
			this.pos = pos;
			this.block = block;
		}
	}
}

public enum ToolType
{
	PENCIL, ERASER, GRAB, FILL, RECT_HOLLOW, RECT_FILL
}
