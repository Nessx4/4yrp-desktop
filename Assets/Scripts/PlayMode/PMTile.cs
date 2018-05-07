using System.Collections;
using System.Collections.Generic;

using UnityEngine;

// Struct to contain the data related to one TileType under one ThemeType.
// Relevant for Play Mode.
public struct PMTile
{
	public readonly PlayTile tilePrefab;

	public PMTile(PlayTile tilePrefab)
	{
		this.tilePrefab = tilePrefab;
	}
}
