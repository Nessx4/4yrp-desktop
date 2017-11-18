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

	// Currently selected tile.
	private TileData activeTile;

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

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && activeTile)
		{
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				Vector3 mousePos = new Vector3(Input.mousePosition.x, 
					Input.mousePosition.y, 10.0f);
				Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);

				pos.x = Mathf.Round(pos.x);
				pos.y = Mathf.Round(pos.y);
				pos.z = 0.0f;

				if (Physics.Raycast(pos, Vector3.up, 0.25f, mask))
				{
					Debug.Log("Already has a tile here.");
					return;
				}

				undoStack.Push(new TileOperation(activeTile.tilePrefab, pos));
				redoStack.Clear();
			}
		}

		if(Input.GetMouseButtonDown(1))
		{
			if(undoStack.Count > 0)
			{
				TileOperation op = undoStack.Pop();

				op.Undo();
				redoStack.Push(op);
			}
		}

		if(Input.GetMouseButtonDown(2))
		{
			if(redoStack.Count > 0)
			{
				TileOperation op = redoStack.Pop();

				op.Redo();
				undoStack.Push(op);
			}
		}
	}

	[System.Serializable]
	private struct TileOperation
	{
		// The instance created by the operation.
		public GameObject newTile;

		// The source instance used by the operation.
		public GameObject tilePrefab;

		// Transform properties.
		public Vector3 position;

		public TileOperation(GameObject tilePrefab, Vector3 position)
		{
			this.tilePrefab = tilePrefab;
			this.position = position;

			newTile = Instantiate(tilePrefab, position, Quaternion.identity);
		}

		public void Undo()
		{
			Destroy(newTile);
		}

		public void Redo()
		{
			newTile = Instantiate(tilePrefab, position, Quaternion.identity);
		}
	}
}
