using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayModeTileData
{
	private Dictionary<ThemeType, PMThemeData> themeDataMap;

	public PlayModeTileData()
	{
		themeDataMap = new Dictionary<ThemeType, PMThemeData>();

		PMThemeData dat = new PMThemeData();

		dat.Add(TileType.NONE, 				new PMTile(null));
		dat.Add(TileType.SOLID, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Solid")));
		dat.Add(TileType.SOLID2,			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Solid2")));
		dat.Add(TileType.SEMISOLID, 		new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Semisolid")));
		dat.Add(TileType.LADDER, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Ladder")));
		dat.Add(TileType.MOVING_PLATFORM, 	new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_MovingPlatform")));
		dat.Add(TileType.TREADMILL, 		new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Treadmill")));
		dat.Add(TileType.START_POINT, 		new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_StartPoint")));
		dat.Add(TileType.CHECK_POINT, 		new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_CheckPoint")));
		dat.Add(TileType.END_POINT, 		new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_EndPoint")));
		dat.Add(TileType.BUSH, 				new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Bush")));
		dat.Add(TileType.CLOUD, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Cloud")));
		dat.Add(TileType.MOUNTAIN, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Mountain")));
		dat.Add(TileType.FLOWER, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Flower")));
		dat.Add(TileType.CRATE, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Crate")));
		dat.Add(TileType.SWEETS, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Sweets")));
		dat.Add(TileType.UFO, 				new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Ufo")));
		dat.Add(TileType.SLIME, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Slime")));
		dat.Add(TileType.SPAWNER, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Spawner")));
		dat.Add(TileType.SPIKES, 			new PMTile(Resources.Load<PlayTile>("TilePrefabs/PlayMode/Normal/obj_Spikes")));

		themeDataMap.Add(ThemeType.NORMAL, dat);
	}

	public PlayTile CreateTile(TileType tileType, ThemeType themeType, 
		Vector2 position)
	{
		if(themeDataMap[themeType][tileType].tilePrefab != null)
			return Object.Instantiate(themeDataMap[themeType][tileType].tilePrefab, 
				position, Quaternion.identity);

		return null;
	}
}
