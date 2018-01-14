using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileDataLoader : MonoBehaviour
{
	private Dictionary<string, TileData> tileData;

	private static TileDataLoader loader = null;

	public static TileDataLoader Get()
	{
		return loader;
	}

	private void Start()
	{
		if (loader == null)
		{
			loader = this;
			DontDestroyOnLoad(gameObject);
			LoadTileData();
		}
		else
			Destroy(gameObject);
	}

	// Parse the Tile Data CSV file.
	private void LoadTileData()
	{
		TextAsset tileDataCSV = Resources.Load("TileData") as TextAsset;
		Debug.Log(tileDataCSV);
		tileData = new Dictionary<string, TileData>();

		string data = tileDataCSV.text;

		// Read line-by-line.
		string[] lines = data.Split('\n');
		for(int i = 1; i < lines.Length; ++i)
		{
			// Get all attributes for the line.
			string[] lineData = lines[i].Split(',');

			if (lineData.Length < 2)
				continue;

			string name = lineData[0].Trim();
			string prefabName = lineData[1].Trim();
			string spriteName = lineData[2].Trim();

			int sizeX;
			int sizeY;
			int.TryParse(lineData[3].Trim(), out sizeX);
			int.TryParse(lineData[4].Trim(), out sizeY);

			string category = lineData[5].Trim();

			CreatorTile creatorPrefab = Resources.Load<CreatorTile>("Tiles/Creator/" + prefabName);
			GameObject runtimePrefab = Resources.Load("Tiles/Runtime/" + prefabName) as GameObject;
			Sprite uiSprite = Resources.Load<Sprite>("Tiles/Palette/" + spriteName);

			TileData newTile = new TileData(name, uiSprite, creatorPrefab, 
				runtimePrefab, new Vector2(sizeX, sizeY), category);

			Debug.Log(name + "1 " + uiSprite + "2 " + creatorPrefab + "3 " + runtimePrefab + "4 " + new Vector2(sizeX, sizeY) + "5 " + category);

			tileData.Add(name, newTile);
		}
	}

	public bool TileExists(string name)
	{
		return tileData.ContainsKey(name);
	}

	public TileData GetData(string name)
	{
		return tileData[name];
	}

	// Get a full list of all TileData.
	public List<TileData> GetAllData()
	{
		Debug.Log(tileData.Values);
		Debug.Log(tileData.Values.Count);
		return new List<TileData>(tileData.Values);
	}
}

public struct TileData
{
	public string name;
	public Sprite uiSprite;
	public CreatorTile creatorPrefab;
	public GameObject runtimePrefab;
	public Vector2 size;
	public string category;

	public TileData(string name, Sprite uiSprite, CreatorTile creatorPrefab, 
		GameObject runtimePrefab, Vector2 size, string category)
	{
		this.name = name;
		this.uiSprite = uiSprite;
		this.creatorPrefab = creatorPrefab;
		this.runtimePrefab = runtimePrefab;
		this.size = size;
		this.category = category;
	}

	public bool IsUnitSize()
	{
		return size.x == 1 && size.y == 1;
	}
}
