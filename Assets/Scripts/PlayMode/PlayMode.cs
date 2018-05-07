using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayMode : MonoBehaviour 
{
	public static PlayMode instance { get; private set; }

	private PlayModeTileData playModeTileData;

	private void Awake()
	{
		playModeTileData = new PlayModeTileData();

		LevelData data = LevelManagement.instance.Load();

		TileType[,] tileTypes = data.tileTypes;
		ThemeType themeType = data.themeType;

		for(int x = 0; x < tileTypes.GetLength(0); ++x)
		{
			for(int y = 0; y < tileTypes.GetLength(1); ++y)
			{
				playModeTileData.CreateTile(tileTypes[x, y], themeType, new Vector2(x, y));
			}
		}
	}
}
