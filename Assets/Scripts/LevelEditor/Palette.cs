using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Palette : MonoBehaviour
{
	public event TileChangedEventHandler TileChanged;

	public delegate void TileChangedEventHandler(object sender, 
		TileChangedEventArgs e);

	protected virtual void OnTileChanged(TileChangedEventArgs e)
	{
		TileChangedEventHandler handler = TileChanged;

		if(handler != null)
			handler(this, e);
	}

	private void Awake()
	{
		// Register self on the LevelEditor service locator.
		LevelEditor.instance.palette = this;
	}

	private void Start()
	{
		// Register an event.
		LevelEditor.instance.toolbar.ToolChanged += ToolChanged;
	}

	private void ToolChanged(object sender, ToolChangedEventArgs e)
	{
		Debug.Log("Event received");

		throw new NotImplementedException("This method will become deprecated");
	}
}

public class TileChangedEventArgs : EventArgs
{
	public readonly TileType tileType;

	public TileChangedEventArgs(TileType tileType)
	{
		this.tileType = tileType;
	}
}
