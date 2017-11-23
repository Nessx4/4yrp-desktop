using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Block : MonoBehaviour 
{
	private Block tilePrefab;

	public Block GetTilePrefab()
	{
		return tilePrefab;
	}

	public void SetTilePrefab(Block tilePrefab)
	{
		this.tilePrefab = tilePrefab;
	}
}
