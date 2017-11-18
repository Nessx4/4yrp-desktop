/*	TileMenu is responsible for populating the sidebar of the LevelCreator scene
 *	with selectable tiles.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TileMenu : MonoBehaviour
{
	// All tiles that can be added by the player.
	[SerializeField]
	private List<Tileset> tiles;
	private int currentSet = 0;
	private int activeInSet = 0;

	// Header denoting the name of the active tileset.
	[SerializeField] 
	private Text tilesetName;

	// A prefab for the selectable tiles that sit on the UI.
	[SerializeField]
	private SelectableTile tileSelectPrefab;

	// UI element that the selectable prefabs reside in.
	[SerializeField]
	private Transform vBox;

	private void Start()
	{
		SwitchTileset(0);
	}

	public void SwitchTileset(bool right)
	{
		currentSet = currentSet + (right ? 1 : -1);

		if(currentSet >= tiles.Count)
			currentSet = 0;
		else if(currentSet < 0)
			currentSet = tiles.Count - 1;

		SwitchTileset(currentSet);
	}

	private void SwitchTileset(int index)
	{
		// Remove any tiles currently present.
		// Somehow this iterates through the children...
		foreach(Transform child in vBox)
		{
			Destroy(child.gameObject);
		}

		// Create a tile selection on the UI for every tile type.
		foreach(var tile in tiles[currentSet].tiles)
		{
			SelectableTile sel = Instantiate(tileSelectPrefab);
			sel.transform.SetParent(vBox);

			sel.SetLinkedItem(tile, this);
		}

		SetActiveTile(tiles[index].tiles[activeInSet]);

		tilesetName.text = tiles[index].name;
	}

	public void SetActiveTile(TileData tile)
	{
		TilePlacement.placement.SetActiveTile(tile);
	}

	[System.Serializable]
	private class Tileset
	{
		public string name;
		public List<TileData> tiles;
	}
}