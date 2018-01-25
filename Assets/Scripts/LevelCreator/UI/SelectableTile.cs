/*	Represents a selectable UI element sitting the sidebar on the LevelCreator 
 *	scene.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(TooltipSource))]
public class SelectableTile : MonoBehaviour
{
	[SerializeField]
	private Color activeColor;

	private Image img;
	private TileData linkedItem;
	private Palette menu;

	private void Awake()
	{
		img = GetComponent<Image>();
		img.color = Color.grey;
	}

	private void Start()
	{
		GetComponent<TooltipSource>().SetTooltipText(linkedItem.name);
	}

	public void SetLinkedItem(TileData linkedItem, Palette menu)
	{
		this.linkedItem = linkedItem;
		this.menu = menu;

		// Also set the sprite for this image to the tile's sprite.
		img.sprite = linkedItem.uiSprite;
	}

	public void OnPress()
	{
		menu.SetActiveTile(linkedItem, this);
	}

	public void Activate()
	{
		img.color = Color.white;
	}

	public void Deactivate()
	{
		img.color = Color.grey;
	}
}