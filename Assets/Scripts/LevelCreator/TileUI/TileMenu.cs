/*	TileMenu is responsible for populating the sidebar of the LevelCreator scene
 *	with selectable tiles.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileMenu : MonoBehaviour
{
	// All tiles that can be added by the player.
	[SerializeField]
	private List<TileData> tiles;

	// A prefab for the selectable tiles that sit on the UI.
	[SerializeField]
	private SelectableTile tileSelectPrefab;

	// UI element that the selectable prefabs reside in.
	[SerializeField]
	private Transform vBox;

	private void Start()
	{
		// Create a tile selection on the UI for every tile type.
		foreach(var tile in tiles)
		{
			SelectableTile sel = Instantiate(tileSelectPrefab);
			sel.transform.SetParent(vBox);

			sel.SetLinkedItem(tile, this);
		}

		SetActiveTile(tiles[0]);
	}

	public void SetActiveTile(TileData tile)
	{
		TilePlacement.placement.SetActiveTile(tile);
	}
}