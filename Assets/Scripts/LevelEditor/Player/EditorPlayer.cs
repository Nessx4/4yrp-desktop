using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class EditorPlayer : MonoBehaviour 
{
	protected TileType activeTile;

	protected DrawState drawState;

	protected abstract IEnumerator Draw();
	protected abstract IEnumerator Erase();
	protected abstract IEnumerator Grab();
}
