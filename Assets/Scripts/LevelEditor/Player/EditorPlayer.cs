using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class EditorPlayer : MonoBehaviour 
{
	protected TileType activeTile;

	protected DrawState drawState;

	protected abstract IEnumerator Draw(DrawState beginState, 
		DrawState endState, TileType tileType);
	//protected abstract IEnumerator Erase();
	protected abstract IEnumerator Grab();
	protected abstract IEnumerator DrawRect(DrawState startState, 
		DrawState endState, bool filled);
}
