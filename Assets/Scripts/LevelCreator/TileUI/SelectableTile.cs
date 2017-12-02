/*	Represents a selectable UI element sitting the sidebar on the LevelCreator 
 *	scene.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SelectableTile : MonoBehaviour
{
	[SerializeField]
	private Color activeColor;

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

	public void OnPress()
	{
		menu.SetActiveTile(linkedItem, this);
	}

	public void Activate()
	{
		img.color = activeColor;
	}

	public void Deactivate()
	{
		img.color = Color.white;
	}
}