/*	LevelEditor is a class that encapsulates state about the Editor. It also
 *	acts as a service locator for several other single-instance classes, such
 *	as Palette or Toolbar. This removes the need to make those classes follow
 *	the Singleton pattern, although this class is forced to.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelEditor
{
	public static LevelEditor instance { get; private set; }

	// LevelEditor acts as a service locator for the following classes:
	public Toolbar toolbar { get; set; }
	public Palette palette { get; set; }

	// Encapsulate all static data in a separate class.
	private EditorTileData editorTileData;

	private ThemeType activeTheme;

	private Dictionary<ThemeType, ThemeData> themeDataMap;

	// The first time instance is requested, it is created.
	static LevelEditor()
	{
		instance = new LevelEditor();
	}

	/* 	On Singleton instance creation, initialise all data about tiles that we
	 *	may want to pull somewhere else in the application.
	 *
	 *	Originally, it was planned to put all static tile data directly inside
	 *	LevelEditor. Instead, this was encapsulated inside a separate object,
	 *	EditorTileData, which is hidden from other classes.
	 */
	private LevelEditor()
	{
		editorTileData = new EditorTileData();
	}

	public GridTile GetTilePrefab(TileType tileType)
	{
		return editorTileData.GetTilePrefab(tileType, activeTheme);
	}

	public Sprite GetPaletteIcon(TileType tileType)
	{
		return editorTileData.GetPaletteIcon(tileType, activeTheme);
	}

	public string GetTileName(TileType tileType)
	{
		return editorTileData.GetTileName(tileType, activeTheme);
	}
}
