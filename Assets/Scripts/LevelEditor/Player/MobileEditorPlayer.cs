using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MobileEditorPlayer : EditorPlayer 
{
	public MobileConnection connection { private get; set; }

	protected override bool CanDrawOverSpace()
	{
		return true;
	}

	protected override GridPosition GetGridPosition()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0.0f;
		mousePos = LevelEditor.instance.mainCamera.ScreenToWorldPoint(mousePos);

		return new GridPosition((int)mousePos.x, (int)mousePos.y);
	}
}
