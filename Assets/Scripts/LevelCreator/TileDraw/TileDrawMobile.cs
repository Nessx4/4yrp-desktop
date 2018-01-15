using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileDrawMobile : TileDraw 
{
	// Check if the Undo and Redo buttons need to be greyed out.
	public override void CheckUndoRedo()
	{
		/*	In the future, this function will send a message back to the
		 *	mobile represented by ths object and tell it to grey out its
		 *	undo/redo buttons if necessary.
		 */
		throw new System.NotImplementedException();
	}
}
