using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Assertions;

//using NUnit.Framework.Internal;

public class SQLTestButton : MonoBehaviour 
{
	[UnityTest]
	public IEnumerator Save_WillSucceedSaving()
	{
		LevelManagement.instance.Save("Test Level");
		yield return null;
	}

	[UnityTest]
	public IEnumerator Load_WillSucceedLoading()
	{
		TileType[,] tiles = LevelManagement.instance.Load(2);
		LevelEditor.instance.editorGrid.SetTileTypes(tiles);
		yield return null;
	}

	[UnityTest]
	public IEnumerator Insert_WillInsertIntoDatabase()
	{
		yield return null;
	}

	[UnityTest]
	public IEnumerator Select_WillSelectFromDatabase()
	{
		yield return null;
	}

	[UnityTest]
	public IEnumerator PrintData()
	{
		yield return null;
	}
}
