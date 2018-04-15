using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelEditor : MonoBehaviour
{
	private ThemeType currentTheme;

	private Dictionary<ThemeType, ThemeData> themeDataMap;

	private void Awake()
	{
		themeDataMap = new Dictionary<ThemeType, ThemeData>();

		ThemeData normal = new ThemeData();
		normal.Add(TileType.SOLID,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Solid"));
		normal.Add(TileType.SEMISOLID,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Semisolid"));
		normal.Add(TileType.LADDER,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Ladder"));
		normal.Add(TileType.MOVING_PLATFORM,	Resources.Load<GridTile>("EditorTiles/Normal/obj_MovingPlatform"));
		normal.Add(TileType.TREADMILL,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Treadmill"));
		normal.Add(TileType.START_POINT,		Resources.Load<GridTile>("EditorTiles/Normal/obj_StartPoint"));
		normal.Add(TileType.CHECK_POINT,		Resources.Load<GridTile>("EditorTiles/Normal/obj_CheckPoint"));
		normal.Add(TileType.END_POINT,			Resources.Load<GridTile>("EditorTiles/Normal/obj_EndPoint"));
		normal.Add(TileType.BUSH,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Bush"));
		normal.Add(TileType.CLOUD,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Cloud"));
		normal.Add(TileType.MOUNTAIN,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Mountain"));
		normal.Add(TileType.CRATE,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Crate"));
		normal.Add(TileType.SWEETS,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Sweets"));
		normal.Add(TileType.UFO,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Ufo"));
		normal.Add(TileType.SLIME,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Slime"));
		normal.Add(TileType.SPAWNER,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Spawner"));
		themeDataMap.Add(ThemeType.NORMAL, normal);
	}
}
