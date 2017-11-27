using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ToolbarButton : MonoBehaviour 
{
	public void Undo()
	{
		TilePlacement.placement.Undo();
	}

	public void Redo()
	{
		TilePlacement.placement.Redo();
	}

	public void Save()
	{
		LevelEditor.editor.Save();
	}

	public void LoadTEMP()
	{
		LevelEditor.editor.Load();
	}

	public void Pencil()
	{
		TilePlacement.placement.SetTool(ToolType.PENCIL);
	}

	public void Eraser()
	{
		TilePlacement.placement.SetTool(ToolType.ERASER);
	}
}
