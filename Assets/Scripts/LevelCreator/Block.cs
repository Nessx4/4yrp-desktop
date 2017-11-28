using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Block : MonoBehaviour 
{
	private Block tilePrefab;

	private BlockType type = BlockType.NORMAL;

	public Block GetTilePrefab()
	{
		return tilePrefab;
	}

	public void SetTilePrefab(Block tilePrefab)
	{
		this.tilePrefab = tilePrefab;
	}

	public BlockType GetBlockType()
	{
		return type;
	}

	public void SetBlockType(BlockType type)
	{
		this.type = type;
	}
}

public enum BlockType
{
	NORMAL, PERMANENT
}
