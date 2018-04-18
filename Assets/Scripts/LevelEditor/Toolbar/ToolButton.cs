using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ToolButton : ToolbarButton 
{
	[SerializeField]
	private ToolType _toolType;

	[SerializeField]
	private Image selectedImage;

	[SerializeField]
	private Color selectedColor;

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

	public override void SetActiveTool(ToolType toolType)
	{
		selectedImage.color = (this.toolType == toolType) ? selectedColor : Color.clear;
	}

	protected override void DoAction()
	{
		toolbar.ChangeTool(toolType);
	}
}
