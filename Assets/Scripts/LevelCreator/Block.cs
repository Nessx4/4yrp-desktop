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
