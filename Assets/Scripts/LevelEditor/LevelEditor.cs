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

	private Toolbar  _toolbar;
	private Palette  _palette;
	private Themebar _themebar;
	public Camera mainCamera { get; set; }
	public EditorGrid editorGrid { get; set; }

	// LevelEditor acts as a service locator for the following classes:
	public Toolbar toolbar   
	{ 
		get
		{
			return _toolbar;
		} 
		set
		{
			_toolbar = value;
			_toolbar.ToolChanged += ToolChanged;
		} 
	}
	public Palette palette 
	{ 
		get
		{
			return _palette;
		}
		set
		{
			_palette = value;
			_palette.TileChanged += TileChanged;
		} 
	}
	public Themebar themebar 
	{ 
		get
		{
			return _themebar;
		}
		set
		{
			_themebar = value;
			_themebar.ThemeChanged += ThemeChanged;
		}
	}

	// Encapsulate all static data in a separate class.
	private EditorTileData editorTileData;

	private ToolType activeTool;
	private TileType activeTile;
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

	private void ToolChanged(object sender, ToolChangedEventArgs e)
	{
		activeTool = e.toolType;
	}

	private void TileChanged(object sender, TileChangedEventArgs e)
	{
		activeTile = e.tileType;
	}

	private void ThemeChanged(object sender, ThemeChangedEventArgs e)
	{
		activeTheme = e.themeType;
	}

	public GridTile GetTilePrefab(TileType tileType)
	{
		return editorTileData.GetTilePrefab(tileType, activeTheme);
	}

	public GridTile CreateTile(TileType type, Vector2 position)
	{
		return editorTileData.CreateTile(type, activeTheme, position);
	}

	public Sprite GetPaletteIcon(TileType tileType)
	{
		return editorTileData.GetPaletteIcon(tileType, activeTheme);
	}

	public string GetTileName(TileType tileType)
	{
		return editorTileData.GetTileName(tileType, activeTheme);
	}

	public Vector2 GetTileSize(TileType tileType)
	{
		return editorTileData.GetTileSize(tileType);
	}

	public bool IsUnitSize(TileType tileType)
	{
		return editorTileData.IsUnitSize(tileType);
	}
}
