/*	A DrawAction controls the state of the Finite State Machine seen in
 *	EditorPlayer and its subclasses. The FSM is used for all drawing operations.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum DrawAction
{
	NONE, DRAW_PENCIL, DRAW_RECT, ERASE, DRAG
}
