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
	private Transform container;

	// Currently selected tile.
	private TileData activeTile;

	// The undo/redo system relies on stacks.
	private Stack<TileOperation> undoStack;
	private Stack<TileOperation> redoStack;

	private Camera mainCam;

	public static TilePlacement placement;

	private void Start()
	{
		placement = this;

		// Camera.main is slow so cache it.
		mainCam = Camera.main;

		undoStack = new Stack<TileOperation>();
		redoStack = new Stack<TileOperation>();
	}

	public void SetActiveTile(TileData tile)
	{
		activeTile = tile;
	}

	public void DeleteUndoHistory()
	{
		undoStack = new Stack<TileOperation>();
		redoStack = new Stack<TileOperation>();
	}

	public Transform GetRoot()
	{
		return container;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && activeTile)
			StartCoroutine(PlaceTiles());

		if(Input.GetMouseButtonDown(1))
			Undo();

		if(Input.GetMouseButtonDown(2))
			Redo();
	}

	private IEnumerator PlaceTiles()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		List<TilePosition> tilePositions = new List<TilePosition>();

		while(Input.GetMouseButton(0))
		{
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				Vector3 mousePos = new Vector3(Input.mousePosition.x, 
					Input.mousePosition.y, 10.0f);
				Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);

				TilePosition tp = new TilePosition(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

				bool hasPlacedHere = false;
				foreach(TilePosition tilePosition in tilePositions)
				{
					if(tilePosition == tp)
					{
						hasPlacedHere = true;
						break;
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
					{
						Debug.Log("HIT");
						existingTile = hitObj.transform.GetComponent<Block>();
					}

					undoStack.Push(new TileOperation(activeTile.tilePrefab, existingTile, pos));
					redoStack.Clear();
				}
			}

			yield return wait;
		}
	}

	public void Undo()
	{
		if (undoStack.Count > 0)
		{
			TileOperation op = undoStack.Pop();

			op.Undo();
			redoStack.Push(op);
		}
	}

	public void Redo()
	{
		if (redoStack.Count > 0)
		{
			TileOperation op = redoStack.Pop();

			op.Redo();
			undoStack.Push(op);
		}
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

		public TileOperation(Block newTilePre, Block oldTileInst, Vector3 position)
		{
			this.newTilePre = newTilePre;
			this.position = position;
			this.oldTileInst = oldTileInst;

			oldTilePre = null;

			if(oldTileInst != null)
			{
				oldTilePre = oldTileInst.GetTilePrefab();
				oldTileInst.gameObject.SetActive(false);
			}

			newTileInst = Instantiate(newTilePre, position, Quaternion.identity, placement.GetRoot());
			newTileInst.SetTilePrefab(newTilePre);
		}

		public void Undo()
		{
			newTileInst.gameObject.SetActive(false);

			if(oldTilePre != null)
				oldTileInst.gameObject.SetActive(true);
		}

		public void Redo()
		{
			if(oldTileInst != null)
			{
				oldTileInst.gameObject.SetActive(false);
			}

			newTileInst.gameObject.SetActive(true);
		}
	}

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
