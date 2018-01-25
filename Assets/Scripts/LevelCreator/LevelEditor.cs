using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LevelEditor : MonoBehaviour 
{
	// The user-input name of the level.
	[SerializeField] 
	private LevelName nameField;

	// Ask the user if they want to overwrite an existing level.
	[SerializeField]
	private OverwriteDialog overwriteDialog;

	// The root Transform all placed tiles are parented to.
	public Transform tileRoot { get; private set; }

	[SerializeField] 
	private WarningMessage warning;

	[SerializeField]
	private Camera mainCam;

	[SerializeField] 
	private PreviewCamera previewCamPrefab;

	public static LevelEditor editor { get; private set; }

	private void Start()
	{
		if(editor == null)
		{
			editor = this;

			tileRoot = new GameObject("TILE_SPAWN_PARENT").transform;

			if (LevelLoader.loader != null)
				Load(LevelLoader.loader.GetLevel());

			// Ensure a snapshot object exists for when we save.
			Instantiate(previewCamPrefab);
		}
		else
			Destroy(gameObject);
	}

	public string GetLevelName()
	{
		return nameField.GetName().Replace(" ", "_").ToLower();
	}

	public void Save(bool overwrite)
	{
		string levelName = nameField.GetName().Replace(" ", "_").ToLower();

		if(levelName == "")
		{
			warning.SetMessage(true, "Name your level!");
			return;
		}

		long timestamp = DateTime.Now.Ticks;

		string fileName = Application.persistentDataPath + "/levels/" + levelName + ".dat";

		// Ensure the Level save folder exists before trying to save a level.
		Directory.CreateDirectory(Application.persistentDataPath + "/levels/");

		if(!overwrite && File.Exists(fileName))
		{
			overwriteDialog.gameObject.SetActive(true);
			overwriteDialog.SetLevelName(nameField.GetName());
		}
		else
		{
			byte[] screenshot = PreviewCamera.cam.TakeScreenshot(mainCam);
			LevelSaveData data = new LevelSaveData(nameField.GetName(), 
				screenshot, DateTime.Now);

			data.name = nameField.GetName();

			foreach (CreatorTile tile in CreatorPlayerWrapper.Get().GetTiles())
			{
				if(tile.gameObject.activeSelf)
					data.tiles.Add(new TileSaveData(tile.GetTileType(), 
						tile.transform.position));
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
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(fileName, FileMode.Open);
			LevelSaveData data = (LevelSaveData)bf.Deserialize(file);
			file.Close();

			nameField.SetName(data.name);

			foreach(TileSaveData tile in data.tiles)
			{
				Vector3 position = new Vector3(tile.positionX, tile.positionY, tile.positionZ);

				CreatorTile prefab = null;

				/*
				switch (tile.type)
				{
					case TileType.SOLID:
						prefab = solid;
						break;
					case TileType.SEMISOLID:
						prefab = semisolid;
						break;
					case TileType.UFO:
						prefab = ufo;
						break;
					case TileType.BUSH01:
						prefab = bush01;
						break;
					case TileType.BUSH02:
						prefab = bush02;
						break;
					case TileType.CLOUD01:
						prefab = cloud01;
						break;
					case TileType.CLOUD02:
						prefab = cloud02;
						break;
					case TileType.MOUNTAIN:
						prefab = mountain;
						break;
					case TileType.CRATE:
						prefab = crate;
						break;
					case TileType.LADDER:
						prefab = ladder;
						break;
					case TileType.DOUGHNUT:
						prefab = doughnut;
						break;
					case TileType.START_POINT:
						prefab = startPoint;
						break;
				}
				*/

				if(prefab != null)
				{
					CreatorTile newTile = Instantiate(prefab, position, Quaternion.identity, tileRoot);
					newTile.SetTilePrefab(prefab);
					CreatorPlayerWrapper.Get().AddTile(newTile);
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

	public byte[] previewImage;

	public long timestamp;

	public List<TileSaveData> tiles;

	public LevelSaveData(string name, byte[] previewImage, DateTime timestamp)
	{
		this.name = name;
		this.previewImage = previewImage;
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
