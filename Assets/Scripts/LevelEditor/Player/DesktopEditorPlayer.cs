using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DesktopEditorPlayer : EditorPlayer 
{
	private void Start()
	{
		LevelEditor.instance.toolbar.ToolChanged += ToolChanged;
		LevelEditor.instance.palette.TileChanged += TileChanged;
	}

	private void ToolChanged(object sender, ToolChangedEventArgs e)
	{

	}

	private void TileChanged(object sender, TileChangedEventArgs e)
	{

	}

	private void Update()
	{
		Vector2 pos = MouseToWorldPoint();

		int x = (int)pos.x;
		int y = (int)pos.y;

		Debug.Log(x + ", " + y);

		if(drawState == DrawState.PENCIL_IDLE)
		{

		}
	}

	protected override IEnumerator Draw()
	{
		yield return null;
	}

	protected override IEnumerator Erase()
	{
		yield return null;
	}

	protected override IEnumerator Grab()
	{
		yield return null;
	}

	private Vector2 MouseToWorldPoint()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0.0f;
		return LevelEditor.instance.mainCamera.ScreenToWorldPoint(mousePos);
	}
}
