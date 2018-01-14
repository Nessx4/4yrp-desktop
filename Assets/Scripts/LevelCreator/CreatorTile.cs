/*	A Block is a tile that gets placed in the Level Editor. This is the component
 *	that is checked for while raycasting to ensure that the thing that gets hit is
 *	a valid tile entity.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CreatorTile : MonoBehaviour 
{
	private CreatorTile tilePrefab;

	[SerializeField]
	private TileType tileType;

	public CreatorTile GetTilePrefab()
	{
		return tilePrefab;
	}

	public void SetTilePrefab(CreatorTile tilePrefab)
	{
		this.tilePrefab = tilePrefab;
	}

	public TileType GetTileType()
	{
		return tileType;
	}
}
