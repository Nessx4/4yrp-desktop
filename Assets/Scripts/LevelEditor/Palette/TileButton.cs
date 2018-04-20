/*	A TileButton exists on the Palette and denotes a tile that can be placed
 *	on the grid.
 */

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TileButton : MonoBehaviour 
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private Image selectedImage;

	public Palette palette { private get; set; }

	public TileType tileType { get; private set; }

	public Color selectedColor { private get; set; }

	public new RectTransform transform { get; private set; }

	private void Awake()
	{
		transform = GetComponent<RectTransform>();
	}

	public void SetTileType(TileType tileType)
	{
		this.tileType = tileType;

		button.image.sprite = LevelEditor.instance.GetPaletteIcon(tileType);
	}

	public void Press()
	{
		palette.SwitchActiveTile(tileType);
	}

	public void SetIsCurrent(bool current)
	{
		//button.image.color = current ? Color.white : Color.grey;
		selectedImage.color = current ? selectedColor : Color.clear;
	}
}
