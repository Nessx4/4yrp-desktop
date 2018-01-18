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
	[SerializeField]
	private Transform tileRoot;

	// All tile prefabs.
	[SerializeField]
	private CreatorTile solid;

	[SerializeField]
	private CreatorTile semisolid;

	[SerializeField]
	private CreatorTile ufo;

	[SerializeField]
	private CreatorTile bush01;

	[SerializeField]
	private CreatorTile bush02;

	[SerializeField]
	private CreatorTile cloud01;

	[SerializeField]
	private CreatorTile cloud02;

	[SerializeField]
	private CreatorTile mountain;

	[SerializeField]
	private CreatorTile crate;

	[SerializeField]
	private CreatorTile ladder;

	[SerializeField]
	private CreatorTile doughnut;

	[SerializeField]
	private CreatorTile startPoint;

	[SerializeField] 
	private WarningMessage warning;

	[SerializeField]
	private Camera mainCamera;

	private Camera previewCamera;

	public static LevelEditor editor;

	private void Start()
	{
		editor = this;

		previewCamera = GetComponent<Camera>();
		previewCamera.enabled = false;

		if (LevelLoader.loader != null)
			Load(LevelLoader.loader.GetLevel());
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
			LevelSaveData data = new LevelSaveData(nameField.GetName(), TakeScreenshot(), DateTime.Now);
			data.name = nameField.GetName();

			foreach (CreatorTile tile in CreatorPlayerWrapper.Get().GetTiles())
			{
				if(tile.gameObject.activeSelf)
					data.tiles.Add(new TileSaveData(tile.GetTileType(), tile.transform.position));
			}

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(fileName);
			bf.Serialize(file, data);
			file.Close();
		}
	}

	private byte[] TakeScreenshot()
	{
		RenderTexture activeTexture = RenderTexture.active;
		RenderTexture tempTexture = new RenderTexture(800, 200, 24);
		//previewCamera.enabled = true;
		previewCamera.targetTexture = tempTexture;
		previewCamera.orthographicSize = 2.5f * mainCamera.aspect;
		previewCamera.aspect = 4.0f;

		// Move the preview camera where it needs to be.
		transform.position = mainCamera.transform.position + new Vector3(0.0f,
			previewCamera.orthographicSize - mainCamera.orthographicSize, 0.0f);

		RenderTexture.active = previewCamera.targetTexture;
		previewCamera.Render();

		Texture2D previewImage = new Texture2D(tempTexture.width, tempTexture.height, 
			TextureFormat.RGB24, false);

		previewImage.ReadPixels(new Rect(0, 0, tempTexture.width, tempTexture.height), 
			0, 0);
		previewImage.Apply();

		RenderTexture.active = activeTexture;
		previewCamera.targetTexture = null;
		Destroy(tempTexture);

		byte[] bytes = previewImage.EncodeToPNG();
		return bytes;
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

public enum TileType
{
	SOLID, SEMISOLID, LADDER,

	START_POINT, CHECK_POINT, WIN_POINT,

	BUSH01, BUSH02, CLOUD01, CLOUD02, MOUNTAIN,

	CRATE,

	DOUGHNUT, CUPCAKE, CANDY_CANE, TOFFEE, LOLLIPOP, STRAWBERRY, ICE_CREAM,
	CHOCOLATE, CANDY_FLOSS, BROWNIE,

	UFO
}
