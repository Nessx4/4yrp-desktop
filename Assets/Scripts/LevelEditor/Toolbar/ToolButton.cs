using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ToolButton : ToolbarButton 
{
	[SerializeField]
	private ToolType toolType;

	protected override void DoAction()
	{
		toolbar.ChangeTool(toolType);
	}
}
