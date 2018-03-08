using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileDataLoader : MonoBehaviour
{
	private Dictionary<string, TileData> tileData;

	public static TileDataLoader instance { get; private set; }

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
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

			EditLayer layer;

			if(!Enum.TryParse(lineData[6].Trim(), false, out layer))
				Debug.Log("AAAAHHHH");

			CreatorTile creatorPrefab = Resources.Load<CreatorTile>("Tiles/Creator/" + prefabName);
			GameObject runtimePrefab = Resources.Load("Tiles/Runtime/" + prefabName) as GameObject;
			Sprite uiSprite = Resources.Load<Sprite>("Tiles/Palette/" + spriteName);

			TileData newTile = new TileData(name, uiSprite, creatorPrefab, 
				runtimePrefab, new Vector2(sizeX, sizeY), category, layer);

			tileData.Add(name, newTile);
		}

		TileData emptyTile = new TileData("Null", null, null, null, Vector2.zero, "", EditLayer.FG);
		tileData.Add("Null", emptyTile);
	}

	public bool TileExists(string name)
	{
		return tileData.ContainsKey(name);
	}

	public TileData GetData(string name)
	{
		return tileData[name];
	}

	public TileData GetData(TileType type)
	{
		switch(type)
		{
			case TileType.SOLID:
				return tileData["Solid Block"];
			case TileType.SEMISOLID:
				return tileData["Semi-solid Block"];
			case TileType.LADDER:
				return tileData["Ladder"];

			case TileType.BUSH01:
				return tileData["Bush (1)"];
			case TileType.BUSH02:
				return tileData["Bush (2)"];
			case TileType.CLOUD01:
				return tileData["Cloud (1)"];
			case TileType.CLOUD02:
				return tileData["Cloud (2)"];
			case TileType.MOUNTAIN:
				return tileData["Mountain"];

			case TileType.CRATE:
				return tileData["Crate"];

			case TileType.DOUGHNUT:
				return tileData["Doughnut"];
			case TileType.CUPCAKE:
				return tileData["Cupcake"];
			case TileType.CANDY_CANE:
				return tileData["Candy Cane"];
			case TileType.TOFFEE:
				return tileData["Toffee"];
			case TileType.LOLLIPOP:
				return tileData["Lollipop"];
			case TileType.STRAWBERRY:
				return tileData["Strawberry"];
			case TileType.ICE_CREAM:
				return tileData["Ice Cream"];
			case TileType.CHOCOLATE:
				return tileData["Chocolate Bar"];
			case TileType.CANDY_FLOSS:
				return tileData["Candy Floss"];
			case TileType.BROWNIE:
				return tileData["Brownie"];

			case TileType.UFO:
				return tileData["UFO"];

			case TileType.START_POINT:
				return tileData["Start Point"];
		}

		return tileData["Null"];
	}

	// Get a full list of all TileData.
	public List<TileData> GetAllData()
	{
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
	public EditLayer layer;

	public TileData(string name, Sprite uiSprite, CreatorTile creatorPrefab, 
		GameObject runtimePrefab, Vector2 size, string category, 
		EditLayer layer)
	{
		this.name = name;
		this.uiSprite = uiSprite;
		this.creatorPrefab = creatorPrefab;
		this.runtimePrefab = runtimePrefab;
		this.size = size;
		this.category = category;
		this.layer = layer;
	}

	public bool IsUnitSize()
	{
		return size.x == 1 && size.y == 1;
	}
}

public enum EditLayer
{
	BG, FG
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
