/*	TileSelect represents one selectable UI element for the sidebar on the
 *	LevelCreator scene.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TileSelect : MonoBehaviour
{
	private Image img;
	private TileData linkedItem;
	private TileMenu menu;

	private void Awake()
	{
		img = GetComponent<Image>();
	}

	public void SetLinkedItem(TileData linkedItem, TileMenu menu)
	{
		this.linkedItem = linkedItem;
		this.menu = menu;

		// Also set the sprite for this image to the tile's sprite.
		img.sprite = linkedItem.tileSprite;
	}

	public void OnPointerEnter()
	{
		Debug.Log("Tooltip: " + linkedItem.name);
		menu.SetActiveTile(linkedItem);
	}
}