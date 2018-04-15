using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ActionButton : ToolbarButton 
{
	[SerializeField]
	private ActionType actionType;

	protected override void DoAction()
	{
		toolbar.ActionPressed(actionType);
	}
}
