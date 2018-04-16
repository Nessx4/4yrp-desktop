using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Palette : MonoBehaviour
{
	[SerializeField]
	private TileButton tileButtonPrefab;

	[SerializeField]
	private RectTransform tileButtonRoot;

	private List<TileButton> tileButtons = new List<TileButton>();

	private List<List<TileType>> tileGroups = new List<List<TileType>>();
	private int tileGroupIndex = -1;

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

		// Create the sets of tiles that will appear on the Palette.
		tileGroups.Add(new List<TileType>() 
		{ 
			TileType.SOLID,
			TileType.SEMISOLID,
			TileType.LADDER,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE
		});

		tileGroups.Add(new List<TileType>()
		{
			TileType.MOVING_PLATFORM,
			TileType.TREADMILL,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE
		});

		tileGroups.Add(new List<TileType>()
		{
			TileType.START_POINT,
			TileType.CHECK_POINT,
			TileType.END_POINT,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE
		});

		tileGroups.Add(new List<TileType>()
		{
			TileType.BUSH,
			TileType.CLOUD,
			TileType.MOUNTAIN,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE
		});

		tileGroups.Add(new List<TileType>()
		{
			TileType.CRATE,
			TileType.SWEETS,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE
		});

		tileGroups.Add(new List<TileType>()
		{
			TileType.SPAWNER,
			TileType.UFO,
			TileType.SLIME,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE,
			TileType.NONE
		});

		// Create an array of 12 buttons on the Palette.
		for(int i = 0; i < 12; ++i)
		{
			TileButton newButton = Instantiate(tileButtonPrefab, tileButtonRoot);
			tileButtons.Add(newButton);
		}
	}

	private void Start()
	{
		// Register an event.
		LevelEditor.instance.toolbar.ToolChanged += ToolChanged;

		SwitchTileGroup();
	}

	private void SwitchTileGroup()
	{
		tileGroupIndex = ++tileGroupIndex % tileGroups.Count;
		Debug.Log(tileGroupIndex);

		int i = 0;
		foreach(var tileButton in tileButtons)
			tileButton.SetTileType(tileGroups[tileGroupIndex][i++]);
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
