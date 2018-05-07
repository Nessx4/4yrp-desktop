using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelRuntime : MonoBehaviour
{
	// All tile prefabs.
	[SerializeField]
	private GameObject solid;

	[SerializeField]
	private GameObject semisolid;

	[SerializeField]
	private GameObject ufo;

	[SerializeField]
	private GameObject bush01;

	[SerializeField]
	private GameObject bush02;

	[SerializeField]
	private GameObject cloud01;

	[SerializeField]
	private GameObject cloud02;

	[SerializeField]
	private GameObject mountain;

	[SerializeField]
	private GameObject crate;

	[SerializeField]
	private GameObject ladder;

	[SerializeField]
	private GameObject doughnut;

	[SerializeField]
	private GameObject startPoint;

	[SerializeField]
	private Transform objectRoot;

	[SerializeField]
	private Player player;

	private void Start()
	{
		if (LevelLoader.instance != null)
			Load(LevelLoader.instance.filename);
	}

	public void Load(string levelName)
	{
		string fileName = Application.persistentDataPath + "/levels/" + levelName + ".dat";

		if (File.Exists(fileName))
		{
			// Empty all objects from the level.
			foreach (Transform t in objectRoot)
				Destroy(t.gameObject);

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(fileName, FileMode.Open);
			LevelSaveData data = (LevelSaveData)bf.Deserialize(file);
			file.Close();

			foreach (TileSaveData tile in data.tiles)
			{
				Vector3 position = new Vector3(tile.positionX, tile.positionY, tile.positionZ);

				GameObject prefab = null;

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
						player.transform.position = position + new Vector3(0.0f, 0.5f, 0.0f);
						break;
				}

				if (prefab != null)
					Instantiate(prefab, position, Quaternion.identity, objectRoot);
			}
		}
		else
			Debug.Log("Nope");
	}
}
