using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TestButton : MonoBehaviour 
{
	public void Save()
	{
		LevelManagement.instance.Save("Test Level");
	}

	public void Load()
	{
		TileType[,] tiles = LevelManagement.instance.Load(2);
		LevelEditor.instance.editorGrid.SetTileTypes(tiles);
	}

	public void PrintData()
	{

	}
}
