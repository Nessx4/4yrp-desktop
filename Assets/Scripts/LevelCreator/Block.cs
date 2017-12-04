/*	A Block is a tile that gets placed in the Level Editor. This is the component
 *	that is checked for while raycasting to ensure that the thing that gets hit is
 *	a valid tile entity.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Block : MonoBehaviour 
{
	private Block tilePrefab;

	[SerializeField]
	private TileType tileType;

	public Block GetTilePrefab()
	{
		return tilePrefab;
	}

	public void SetTilePrefab(Block tilePrefab)
	{
		this.tilePrefab = tilePrefab;
	}

	public TileType GetTileType()
	{
		return tileType;
	}
}
