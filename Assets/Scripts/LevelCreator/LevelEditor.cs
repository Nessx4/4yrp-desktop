using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class LevelEditor : MonoBehaviour 
{
	// The user-input name of the level.
	[SerializeField] 
	private LevelName nameField;

	// The root Transform all placed tiles are parented to.
	[SerializeField]
	private Transform tileRoot;

	[SerializeField]
	private GameObject solid;

	[SerializeField]
	private GameObject semisolid;

	[SerializeField] 
	private WarningMessage warning;

	public static LevelEditor editor;

	private void Start()
	{
		editor = this;
	}

	public void Save()
	{
		string levelName = nameField.GetName().Replace(" ", "_").ToLower();

		if(levelName == "")
		{
			warning.SetMessage(true, "Name your level!");
			return;
		}

		string fileName = Application.persistentDataPath + "/levels/" + levelName + ".dat";

		// Ensure the Level save folder exists before trying to save a level.
		Directory.CreateDirectory(Application.persistentDataPath + "/levels/");

		if(File.Exists(fileName))
		{
			Debug.LogError("Later, this will need to bring up a prompt asking to overwrite.");
		}
		//else
		//{
			LevelSaveData data = new LevelSaveData("");

			foreach (Transform tile in tileRoot)
				data.tiles.Add(new TileSaveData(TileType.SOLID, tile.position));

			Debug.Log(data.tiles);

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(fileName);
			bf.Serialize(file, data);
			file.Close();
		//}
	}

	public void Load()
	{
		string levelName = nameField.GetName().Replace(" ", "_").ToLower();

		if (levelName == "")
		{
			warning.SetMessage(true, "Level does not exist!");
			return;
		}

		string fileName = Application.persistentDataPath + "/levels/" + levelName + ".dat";

		if(File.Exists(fileName))
		{
			foreach (Transform tile in tileRoot)
				Destroy(tile.gameObject);

			TilePlacement.placement.DeleteUndoHistory();

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(fileName, FileMode.Open);
			LevelSaveData data = (LevelSaveData)bf.Deserialize(file);
			file.Close();

			Debug.Log(data.tiles);

			foreach(TileSaveData tile in data.tiles)
			{
				Vector3 position = new Vector3(tile.positionX, tile.positionY, tile.positionZ);
				if (tile.type == TileType.SOLID)
					Instantiate(solid, position, Quaternion.identity, tileRoot);
				else
					Instantiate(semisolid, position, Quaternion.identity, tileRoot);
			}
		}
		else
		{
			warning.SetMessage(true, "Level does not exist!");
		}
	}
}

[System.Serializable]
public struct LevelSaveData
{
	public string name;

	public List<TileSaveData> tiles;

	public LevelSaveData(string name)
	{
		this.name = name;
		tiles = new List<TileSaveData>();
	}
}

[System.Serializable]
public struct TileSaveData
{
	public TileType type;
	public float positionX;
	public float positionY;
	public float positionZ;

	public TileSaveData(TileType type, Vector3 position)
	{
		this.type = type;
		positionX = position.x;
		positionY = position.y;
		positionZ = position.z;
	}
}

public enum TileType
{
	SOLID, SEMISOLID
}
