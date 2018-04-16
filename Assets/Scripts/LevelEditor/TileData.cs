using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Struct to contain the data related to one TileType under one ThemeType.
public struct TileData
{
	public readonly GridTile gridPrefab;
	public readonly Sprite paletteSprite;
	public readonly string name;

	public TileData(GridTile gridPrefab, Sprite paletteSprite, string name)
	{
		this.gridPrefab = gridPrefab;
		this.paletteSprite = paletteSprite;
		this.name = name;
	}
}
