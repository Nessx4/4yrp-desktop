/*	Controls the placement of tiles into the world and the undo/redo function.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacement : MonoBehaviour 
{
	// The tile placement raycast only looks at certain layers.
	[SerializeField]
	private LayerMask mask;

	// Currently selected tile.
	private TileData activeTile;

	private Stack<TileOperation> operations;

	private Camera mainCam;

	public static TilePlacement placement;

	private void Start()
	{
		placement = this;
		// Camera.main is slow so cache it.
		mainCam = Camera.main;

		operations = new Stack<TileOperation>();
	}

	public void SetActiveTile(TileData tile)
	{
		activeTile = tile;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && activeTile)
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

			GameObject newTile = Instantiate(activeTile.tilePrefab, pos, 
				Quaternion.identity);

			operations.Push(new TileOperation(newTile, activeTile.tilePrefab, pos));
		}

		if(Input.GetMouseButtonDown(1))
		{
			TileOperation op = operations.Pop();

			Destroy(op.newTile);
		}
	}

	[System.Serializable]
	private struct TileOperation
	{
		public GameObject newTile;

		public GameObject tilePrefab;
		public Vector3 position;

		public TileOperation(GameObject newTile, GameObject tilePrefab, 
			Vector3 position)
		{
			this.newTile = newTile;
			this.tilePrefab = tilePrefab;
			this.position = position;
		}
	}
}
