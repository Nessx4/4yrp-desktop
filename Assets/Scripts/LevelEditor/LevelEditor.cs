using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelEditor
{
	public static LevelEditor instance { get; private set; }

	public Toolbar toolbar { get; set; }
	public Palette palette { get; set; }

	private ThemeType currentTheme;

	private Dictionary<ThemeType, ThemeData> themeDataMap;

	// The first time instance is requested, it is created.
	static LevelEditor()
	{
		instance = new LevelEditor();
	}

	// On Singleton instance creation, 
	private LevelEditor()
	{
		themeDataMap = new Dictionary<ThemeType, ThemeData>();

		ThemeData dat = new ThemeData();
		dat.Add(TileType.SOLID,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Solid"));
		dat.Add(TileType.SEMISOLID,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Semisolid"));
		dat.Add(TileType.LADDER,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Ladder"));
		dat.Add(TileType.MOVING_PLATFORM,	Resources.Load<GridTile>("EditorTiles/Normal/obj_MovingPlatform"));
		dat.Add(TileType.TREADMILL,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Treadmill"));
		dat.Add(TileType.START_POINT,		Resources.Load<GridTile>("EditorTiles/Normal/obj_StartPoint"));
		dat.Add(TileType.CHECK_POINT,		Resources.Load<GridTile>("EditorTiles/Normal/obj_CheckPoint"));
		dat.Add(TileType.END_POINT,			Resources.Load<GridTile>("EditorTiles/Normal/obj_EndPoint"));
		dat.Add(TileType.BUSH,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Bush"));
		dat.Add(TileType.CLOUD,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Cloud"));
		dat.Add(TileType.MOUNTAIN,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Mountain"));
		dat.Add(TileType.CRATE,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Crate"));
		dat.Add(TileType.SWEETS,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Sweets"));
		dat.Add(TileType.UFO,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Ufo"));
		dat.Add(TileType.SLIME,				Resources.Load<GridTile>("EditorTiles/Normal/obj_Slime"));
		dat.Add(TileType.SPAWNER,			Resources.Load<GridTile>("EditorTiles/Normal/obj_Spawner"));
		themeDataMap.Add(ThemeType.NORMAL, dat);
	}
}
