/*	A wrapper for the TileDraw class that will read inputs and events from the
 *	desktop player and upto four mobiles and draw tiles accordingly.
 *
 *	An ID of 0 is the desktop player, IDs 1-4 are mobile players.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileDrawWrapper : MonoBehaviour
{
	// The tile placement raycast only looks at certain layers.
	[SerializeField]
	private LayerMask mask;

	// List of all tiles placed in the level so far.
	private List<CreatorTile> tiles;

	// Objects that encapsulate tile drawing functions.
	[SerializeField]
	private List<TileDraw> tileDraws;

	[SerializeField] 
	private TileDraw mobileDrawPre;

	private Transform spawnRoot;

	private static TileDrawWrapper wrapper;

	public static TileDrawWrapper Get()
	{
		return wrapper;
	}

	private void Start()
	{
		wrapper = this;
		spawnRoot = new GameObject().transform;

		tiles = new List<CreatorTile>();

		if(tileDraws.Count != 1)
			Debug.LogError("Tile draw object for desktop must be in scene at start.");

		tileDraws[0].SetParameters(this, 0, spawnRoot, mask);
	}

	public void RegisterMobile()
	{
		TileDraw newTileDraw = Instantiate(mobileDrawPre, Vector3.zero, 
			Quaternion.identity) as TileDraw;

		tileDraws.Add(newTileDraw);

		// Set the wrapper object, ID and spawned tile root transform;
		newTileDraw.SetParameters(this, tileDraws.Count - 1, spawnRoot, mask);
	}

	private void Update()
	{
		/*	To do in Update:
		 *		-> Update tile preview positions.
		 *		-> Poll for desktop mouse pos and click events etc.
		 *		-> Poll for mobile events.
		 *		-> Set movement delta for each cursor.
		 */
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

	// Get the Transform that all spawned tiles are parented to.
	public Transform GetRoot()
	{
		return spawnRoot;
	}

	public void SetUndoRedo(int id)
	{

	}

	public void SetActiveTile(int id, TileData tile)
	{
		tileDraws[id].SetActiveTile(tile);
	}

	public void SetActiveTool(int id, ToolType tool)
	{
		tileDraws[id].SetActiveTool(tool);
	}

	// Please implement this by Sunday.
	public void ReceiveMessagesFromWhateverHughsStuffDoes()
	{

	}

	public void Undo(int id)
	{

	}

	public void Redo(int id)
	{

	}

	public void Clear(int id)
	{
		
	}
}
