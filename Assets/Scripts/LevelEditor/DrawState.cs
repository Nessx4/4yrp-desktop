/*	A DrawState controls the state of the Finite State Machine seen in
 *	EditorPlayer and its subclasses. The FSM is used for all drawing operations.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum DrawState
{
	PENCIL_IDLE, PENCIL_DRAW, 
	ERASER_IDLE, ERASER_DRAW, 
	GRAB_IDLE, GRAB_SELECT, GRAB_DRAG,
	RECT_HOLLOW_IDLE, RECT_HOLLOW_DRAW,
	RECT_FILL_IDLE, RECT_FILL_DRAW
}
