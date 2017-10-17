/*	TileMenu is responsible for populating the sidebar of the LevelCreator scene
 *	with selectable tiles.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMenu : MonoBehaviour
{
	[SerializeField]
	private List<TileData> tiles;

	[SerializeField]
	private TileSelect tileSelectPrefab;

	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private Transform vBox;

	private Camera mainCam;
	private TileData activeTile;

	private void Start()
	{
		// Camera.main is slow so cache it.
		mainCam = Camera.main;

		// Create a tile selection on the UI for every tile type.
		foreach(var tile in tiles)
		{
			TileSelect sel = Instantiate(tileSelectPrefab);
			sel.transform.SetParent(vBox);

			sel.SetLinkedItem(tile, this);
		}

		activeTile = tiles[0];
	}

	public void SetActiveTile(TileData tile)
	{
		activeTile = tile;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && activeTile)
		{
			Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
			Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);

			pos.x = Mathf.Round(pos.x);
			pos.y = Mathf.Round(pos.y);
			pos.z = 0.0f;

			if (Physics.Raycast(pos, Vector3.up, 0.25f, mask))
			{
				Debug.Log("Already has a tile here.");
				return;
			}

			GameObject newTile = Instantiate(activeTile.tilePrefab, pos, Quaternion.identity);
		}
	}
}