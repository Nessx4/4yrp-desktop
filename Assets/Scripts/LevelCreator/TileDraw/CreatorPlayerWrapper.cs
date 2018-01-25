/*	A wrapper for the TileDraw class that will read inputs and events from the
 *	desktop player and upto four mobiles and draw tiles accordingly.
 *
 *	An ID of 0 is the desktop player, IDs 1-4 are mobile players.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CreatorPlayerWrapper : MonoBehaviour
{
	// The tile placement raycast only looks at certain layers.
	[SerializeField]
	private LayerMask mask;

	// List of all tiles placed in the level so far.
	private List<CreatorTile> tiles;

	// Objects that encapsulate tile drawing functions.
	[SerializeField]
	private List<CreatorPlayer> players;

	[SerializeField] 
	private CreatorPlayerMobile mobilePlayerPre;

	[SerializeField] 
	private TileData defaultTile;

	private static CreatorPlayerWrapper wrapper;

	public static CreatorPlayerWrapper Get()
	{
		return wrapper;
	}

	private void Start()
	{
		wrapper = this;

		tiles = new List<CreatorTile>();

		if(players.Count != 1)
			Debug.LogError("Desktop tile draw object must exist at start.");

		players[0].SetParameters(this, 0, LevelEditor.editor.tileRoot, mask);
	}

	// Find a player by ID.
	public CreatorPlayer GetPlayer(int id)
	{
		return players[id];
	}

	public void RegisterMobile()
	{
		CreatorPlayer newPlayer = Instantiate(mobilePlayerPre, Vector3.zero, 
			Quaternion.identity) as CreatorPlayer;

		players.Add(newPlayer);

		// Set the wrapper object, ID and spawned tile root transform;
		newPlayer.SetParameters(this, players.Count - 1, 
			LevelEditor.editor.tileRoot, mask);
	}

	// Add a tile to the list of tiles.
	public void AddTile(CreatorTile tile)
	{
		tiles.Add(tile);
	}

	// Return a list of all tiles added to the level.
	public List<CreatorTile> GetTiles()
	{
		return tiles;
	}

	public void SetActiveTile(int id, TileData tile)
	{
		players[id].SetActiveTile(tile);
	}

	public void SetActiveTool(int id, ToolType tool)
	{
		players[id].SetActiveTool(tool);
	}

	public void Undo(int id)
	{
		players[id].Undo();
	}

	public void Redo(int id)
	{
		players[id].Redo();
	}

	public void Clear(int id)
	{
		players[id].ClearAll();
	}
}
