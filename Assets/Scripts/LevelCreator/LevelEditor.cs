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
	//[SerializeField] 
	//private LevelName nameField;

	[SerializeField]
	private SaveDialog saveDialog;

	// Ask the user if they want to overwrite an existing level.
	//[SerializeField]
	//private OverwriteDialog overwriteDialog;

	// The root Transform all placed tiles are parented to.
	public Transform tileRoot { get; private set; }

	public string levelName { get; private set; }
	public string filename  { get; private set; }

	[SerializeField] 
	private WarningMessage warning;

	[SerializeField]
	private Camera mainCam;

	[SerializeField] 
	private PreviewCamera previewCamPrefab;

	public static LevelEditor instance { get; private set; }

	private void Start()
	{
		if(instance == null)
		{
			instance = this;

			tileRoot = new GameObject("TILE_SPAWN_PARENT").transform;

			filename = null;
			if (LevelLoader.instance != null)
				Load(LevelLoader.instance.filename);

			// Ensure a snapshot object exists for when we save.
			Instantiate(previewCamPrefab);
		}
		else
			Destroy(gameObject);
	}

	public void Save(bool overwrite)
	{
		string levelName = "New level";//nameField.GetName().Replace(" ", "_").ToLower();

		if(levelName == "")
		{
			warning.SetMessage(true, "Name your level!");
			return;
		}

		long timestamp = DateTime.Now.Ticks;

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath +
			"/levels/db.dat", FileMode.Open);
		LevelDatabase db = (LevelDatabase)bf.Deserialize(file);
		file.Close();

		string new_filename = Application.persistentDataPath + "/levels/";

		if(filename == null) 
		{
			int n = db.lastID++;
			new_filename += (n.ToString() + ".dat");
		}
		else
			new_filename += (filename + ".dat");

		// Ensure the Level save folder exists before trying to save a level.
		Directory.CreateDirectory(Application.persistentDataPath + "/levels/");

		if(!overwrite && File.Exists(new_filename))
		{
			//overwriteDialog.gameObject.SetActive(true);
			//overwriteDialog.SetLevelName(nameField.GetName());
		}
		else
		{
			byte[] screenshot = PreviewCamera.cam.TakeScreenshot(mainCam);
			LevelSaveData data = new LevelSaveData(levelName, 
				screenshot, DateTime.Now);

			data.name = levelName;

			foreach (CreatorTile tile in CreatorPlayerWrapper.Get().GetTiles())
			{
				if(tile.gameObject.activeSelf)
					data.tiles.Add(new TileSaveData(tile.GetTileType(), 
						tile.transform.position));
			}

			file = File.Create(new_filename);
			bf.Serialize(file, data);
			file.Close();

			db.AddLevel(new_filename, levelName);
			file = File.Open(Application.persistentDataPath +
				"/levels/db.dat", FileMode.Open);
			bf.Serialize(file, db);
			file.Close();
		}
	}

	public void Load(string filename)
	{
		this.filename = filename;

		BinaryFormatter bf = new BinaryFormatter();
		string file_to_open = Application.persistentDataPath + "/levels/db.dat";

		if(File.Exists(file_to_open))
		{
			FileStream file = File.Open(file_to_open, FileMode.Open);
			LevelDatabase db = (LevelDatabase)bf.Deserialize(file);
			file.Close();

			// Find this level's filename and level name.
			levelName = db.names[filename];

			file_to_open = Application.persistentDataPath + "/levels/" +
				filename + ".dat";

			if(File.Exists(file_to_open))
			{
				file = File.Open(file_to_open, FileMode.Open);
				LevelSaveData data = (LevelSaveData)bf.Deserialize(file);
				file.Close();

				Debug.Log("Loading level: " + levelName);
				//levelName = data.name;

				foreach(TileSaveData tile in data.tiles)
				{
					Vector3 position = new Vector3(tile.positionX, 
						tile.positionY, tile.positionZ);

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
						CreatorTile newTile = Instantiate(prefab, position, 
							Quaternion.identity, tileRoot);
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
		else
			Debug.Log("Database missing!");
	}
}

// Each time a new level is created, increment the ID and give it the filename
// "<new ID>.dat".
[Serializable]
public struct LevelDatabase
{
	// Filename => level name.
	public Dictionary<string, string> names;
	public int lastID;

	public LevelDatabase(Dictionary<string, string> names)
	{
		lastID = 0;
		this.names = names;
	}

	public void AddLevel(string filename, string levelName)
	{
		if(names == null)
			names = new Dictionary<string, string>();

		if(names.ContainsKey(filename))
			names[filename] = levelName;
		else
			names.Add(filename, levelName);
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
