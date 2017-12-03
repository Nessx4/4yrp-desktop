using System;
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
	private Block solid;

	[SerializeField]
	private Block semisolid;

	[SerializeField] 
	private WarningMessage warning;

	public static LevelEditor editor;

	private void Start()
	{
		editor = this;

		if(LevelLoader.loader != null)
			Load(LevelLoader.loader.GetLevel());
	}

	public void Save(bool overwrite)
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

		if(!overwrite && File.Exists(fileName))
		{
			Debug.LogError("Later, this will need to bring up a prompt asking to overwrite.");
		}
		//else
		{
			LevelSaveData data = new LevelSaveData("", DateTime.Now);
			data.name = nameField.GetName();

			foreach (Block block in TilePlacement.placement.GetBlocks())
			{
				if(block.gameObject.activeSelf)
					data.tiles.Add(new TileSaveData(block.GetTileType(), block.transform.position));
			}

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(fileName);
			bf.Serialize(file, data);
			file.Close();
		}
	}

	public void Load(string levelName)
	{
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

			nameField.SetName(data.name);

			foreach(TileSaveData tile in data.tiles)
			{
				Vector3 position = new Vector3(tile.positionX, tile.positionY, tile.positionZ);

				Block prefab = null;

				switch (tile.type)
				{
					case TileType.SOLID:
						prefab = solid;
						break;
					case TileType.SEMISOLID:
						prefab = semisolid;
						break;
					case TileType.UFO:
						break;
					case TileType.BUSH01:
						break;
					case TileType.BUSH02:
						break;
					case TileType.CLOUD01:
						break;
					case TileType.CLOUD02:
						break;
					case TileType.MOUNTAIN:
						break;
				}

				if(prefab != null)
				{
					Block newBlock = Instantiate(prefab, position, Quaternion.identity, tileRoot);
					newBlock.SetTilePrefab(prefab);
					TilePlacement.placement.AddBlock(newBlock);
				}
			}
		}
		else
		{
			warning.SetMessage(true, "Level does not exist!");
		}
	}
}

[Serializable]
public struct LevelSaveData
{
	public string name;

	public long timestamp;

	public List<TileSaveData> tiles;

	public LevelSaveData(string name, DateTime timestamp)
	{
		this.name = name;
		this.timestamp = timestamp.Ticks;
		tiles = new List<TileSaveData>();
	}
}

[Serializable]
public class TileSaveData
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
	SOLID, SEMISOLID, UFO, BUSH01, BUSH02, CLOUD01, CLOUD02, MOUNTAIN
}
