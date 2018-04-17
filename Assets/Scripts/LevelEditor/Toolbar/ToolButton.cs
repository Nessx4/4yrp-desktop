using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ToolButton : ToolbarButton 
{
	[SerializeField]
	private ToolType _toolType;

	public ToolType toolType
	{
		get
		{
			return _toolType;
		}
		private set
		{
			_toolType = value;
		}
	}

	protected sealed override void Awake()
	{
		base.Awake();
	}

	protected override void DoAction()
	{
		toolbar.ChangeTool(toolType);
	}
}
