using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class LevelEditor : MonoBehaviour 
{
	[SerializeField] 
	private LevelName nameField;

	public void Save()
	{
		string levelName = nameField.GetName().Replace(" ", "_").ToLower();

		if(levelName == "")
		{
			Debug.LogError("Invalid filename; please fill in the name field.");
			return;
		}

		string fileName = Application.persistentDataPath + "/levels/" + levelName + ".dat";

		if(File.Exists(fileName))
		{
			Debug.LogError("Later, this will need to bring up a prompt asking to overwrite.");
		}
		else
		{
			LevelSaveData data = new LevelSaveData("");

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(fileName);
			bf.Serialize(file,data);
			file.Close();
		}
	}
}

public struct LevelSaveData
{
	public string name;

	public LevelSaveData(string name)
	{
		this.name = name;
	}
}

public struct TileSaveData
{

}
