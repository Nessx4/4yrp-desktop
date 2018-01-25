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
	private Image img;
	private TileData linkedItem;
	private Palette menu;

	private Color activeColor = new Color(0.85f, 0.55f, 0.77f, 1.0f);
	private Color inactiveColor = new Color(0.75f, 0.75f, 0.75f, 1.0f);

	private void Awake()
	{
		img = GetComponent<Image>();
		img.color = Color.white;
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
		img.color = activeColor;
	}

	public void Deactivate()
	{
		img.color = Color.white;
	}
}