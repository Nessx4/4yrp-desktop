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

	// List of all blocks placed in the level so far.
	private List<CreatorTile> blocks;

	// Objects that encapsulate tile drawing functions.
	private List<TileDraw> tileDraws;

	[SerializeField] 
	private TileDraw tileDrawDesktop;

	[SerializeField] 
	private TileDraw tileDrawMobile;

	private Transform spawnRoot;

	private void Start()
	{
		spawnRoot = new GameObject().transform;

		tileDraws = new List<TileDraw>():
		blocks = new List<CreatorTile>();

		RegisterController(false);
	}

	public void RegisterController(bool mobile)
	{
		TileDraw newTileDraw = 
			Instantiate((mobile ? tileDrawMobile : tileDrawDesktop), 
				Vector3.zero, Quaternion.identity) as TileDraw;


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

	public void SetUndoRedo(int id)
	{

	}

	public void SetActiveTile(int id, TileData tile)
	{
		bool show = tileDraws[id].SetActiveTile(tile);
		if(id == 0)
		{
			fillButton.SetVisible(true);
			rectFilledButton.SetVisible(true);
			rectHollowButton.SetVisible(true);
		}
		else
		{
			fillButton.SetVisible(false);
			rectFilledButton.SetVisible(false);
			rectHollowButton.SetVisible(false);
		}
	}

	public void Undo(int id)
	{

	}

	public void Redo(int id)
	{

	}
}
