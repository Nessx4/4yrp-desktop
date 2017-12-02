/*	TileMenu is responsible for populating the sidebar of the LevelCreator scene
 *	with selectable tiles.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
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

	// The currently selected tile.
	private SelectableTile activeTile;

	private List<SelectableTile> visibleTiles;

	[SerializeField] 
	private RectTransform dropDownArrow;

	private bool extended = true;

	private new RectTransform transform;

	private void Start()
	{
		transform = GetComponent<RectTransform>();

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
			Destroy(child.gameObject);

		visibleTiles = new List<SelectableTile>();

		// Create a tile selection on the UI for every tile type.
		foreach(var tile in tiles[currentSet].tiles)
		{
			SelectableTile sel = Instantiate(tileSelectPrefab);
			sel.transform.SetParent(vBox, false);
			visibleTiles.Add(sel);

			sel.SetLinkedItem(tile, this);
		}

		SetActiveTile(tiles[index].tiles[activeInSet], visibleTiles[0]);

		tilesetName.text = tiles[index].name;
	}

	public void ToggleExtended()
	{
		extended = !extended;
		dropDownArrow.rotation = Quaternion.Euler(0.0f, 0.0f, extended ? 90.0f : -90.0f);
		transform.offsetMin = new Vector2(transform.offsetMin.x, extended ? 25 : 850);
	}

	public void SetActiveTile(TileData data, SelectableTile tile)
	{
		TilePlacement.placement.SetActiveTile(data);

		if(activeTile != null)
			activeTile.Deactivate();

		activeTile = tile;
		activeTile.Activate();
	}

	[System.Serializable]
	private class Tileset
	{
		public string name;
		public List<TileData> tiles;
	}
}