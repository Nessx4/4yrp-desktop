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
	private Block solid;

	[SerializeField]
	private Block semisolid;

	[SerializeField]
	private Block ufo;

	[SerializeField]
	private Block bush01;

	[SerializeField]
	private Block bush02;

	[SerializeField]
	private Block cloud01;

	[SerializeField]
	private Block cloud02;

	[SerializeField]
	private Block mountain;

	[SerializeField]
	private Block crate;

	[SerializeField]
	private Block ladder;

	[SerializeField]
	private Block doughnut;

	[SerializeField]
	private Block startPoint;

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
	SOLID, SEMISOLID, UFO,
	BUSH01, BUSH02, CLOUD01, CLOUD02, MOUNTAIN,
	START_POINT, CHECK_POINT, WIN_POINT,
    CRATE, LADDER, DOUGHNUT
}
