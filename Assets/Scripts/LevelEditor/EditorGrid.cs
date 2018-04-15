/*	The EditorGrid is a single-instance class that controls the Model portion
 *	of an MVC system comprised of: 
 *
 *		the EditorPlayer class (and subclasses) - the Controller; 
 *
 *		the set of GameObjects, specifically EditorTiles, that are visible 
 *		on-screen - the View; 
 *
 *		and this class - an abstract data structure denoting the tiles at each
 *		grid space - the Model.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EditorGrid : MonoBehaviour
{
	private GridTile[,] gridTiles;

	private void Awake()
	{
		gridTiles = new GridTile[100, 100];
	}

	// Replace the tile at (x, y) with a different type if applicable.
	private void UpdateSpace(int x, int y)
	{
		
	}

	// Update the contents of every tile with the correctly-themed GridTile.
	private void UpdateAllTiles()
	{

	}

	private void ClearAllTiles()
	{

	}
}
