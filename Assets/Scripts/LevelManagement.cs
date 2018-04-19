/*	LevelManagement encapsulates all the functionality required for saving
 *	and loading a level. 
 *
 *	Additionally, it keeps track of the level that will next need to be loaded 
 *	across scenes, for example when a user selects a level from the Browser or 
 *	creates a new one (an ID of -1 is assigned by default).
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class LevelManagement 
{
	public static long id;
}
