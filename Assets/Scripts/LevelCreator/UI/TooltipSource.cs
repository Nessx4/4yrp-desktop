/*	The UI element attached to this script is a tool or button which
 *	needs to display a helpful description on the tooltip.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TooltipSource : MonoBehaviour
{
	[SerializeField]
	private string tooltipText;

	// The mouse pointer enters the element.
	public void Enter()
	{
		Tooltip.tooltip.StartTip(tooltipText);
	}

	// The mouse pointer exits the element.
	public void Exit()
	{
		Tooltip.tooltip.EndTip();
	}

	// Change the text this UI element will display.
	public void SetTooltipText(string tooltipText)
	{
		this.tooltipText = tooltipText;
	}
}
