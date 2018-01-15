using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour 
{
	[SerializeField]
	private Color activeColor;

	[SerializeField]
	private ToolbarButton activeTool;

	private void Start()
	{
		activeTool.SetColor(activeColor);
	}

	// Set a tool directly from a toolbar button.
	public void SetTool(ToolbarButton newTool)
	{
		activeTool.SetColor(Color.white);
		activeTool = newTool;
		activeTool.SetColor(activeColor);

		TileDraw.placement.SetActiveTool(activeTool.GetTool());
	}
}
