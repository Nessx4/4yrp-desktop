using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public struct LevelData
{
	public readonly TileType[,] tileTypes;
	public readonly ThemeType themeType;

	/*
	public LevelData()
	{
		tileTypes = new TileType[100, 100];
		themeType = ThemeType.NORMAL;
	}
	*/

	public LevelData(TileType[,] tileTypes, ThemeType themeType)
	{
		this.tileTypes = tileTypes;
		this.themeType = themeType;
	}
}
