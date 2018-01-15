/*	A wrapper for the TileDraw class that will read inputs and events from the
 *	desktop player and upto four mobiles and draw tiles accordingly.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileDrawWrapper : MonoBehaviour
{
	// The tile placement raycast only looks at certain layers.
	[SerializeField]
	private LayerMask mask;

	[SerializeField]
	private ToolbarButton undoButton;

	[SerializeField]
	private ToolbarButton redoButton;

	[SerializeField]
	private ToolbarButton fillButton;

	[SerializeField]
	private ToolbarButton rectFilledButton;

	[SerializeField]
	private ToolbarButton rectHollowButton;

	private List<TileDraw> tileDraws;

	private void Start()
	{
		/*	To do in Start:
		 *		-> Send mask to TileDraw.
		 *		-> Create and set spawn parent on TileDraw.
		 */
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

	public void SetActiveTile(int id, TileData tile)
	{
		if(tileDraws[id].SetActiveTile(tile))
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
