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

	public Palette palette { protected get; set; }

	private TileType tileType;

	public new RectTransform transform { get; private set; }

	private void Awake()
	{
		transform = GetComponent<RectTransform>();
	}

	public void SetTileType(TileType tileType)
	{
		this.tileType = tileType;

		button.image.sprite = LevelEditor.instance.GetPaletteIcon(tileType);

		//throw new NotImplementedException("Need to change the tile sprite etc");
	}

	public void Press()
	{

	}
}
