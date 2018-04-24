using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TestButtons : MonoBehaviour 
{
	public void TestSave()
	{
		LevelManagement.instance.Save("My level", "My description");
	}

	public void TestLoad()
	{
		LevelData data = LevelManagement.instance.Load(2);

		LevelEditor.instance.editorGrid.SetTileTypes(data.tileTypes);
		LevelEditor.instance.themebar.activeTheme = data.themeType;
	}
}
