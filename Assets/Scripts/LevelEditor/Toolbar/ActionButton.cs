using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ActionButton : ToolbarButton 
{
	[SerializeField]
	private ActionType _actionType;

	public ActionType actionType
	{
		get
		{
			return _actionType;
		}
		private set
		{
			_actionType = value;
		}
	}

	protected override void DoAction()
	{
		toolbar.ActionPressed(actionType);
	}
}
