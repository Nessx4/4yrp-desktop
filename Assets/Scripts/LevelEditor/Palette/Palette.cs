using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

public class Palette : MonoBehaviour
{
	[SerializeField]
	private TileButton tileButtonPrefab;

	[SerializeField]
	private RectTransform tileButtonRoot;

	[SerializeField]
	private Color[] selectedColors;

	private List<TileButton> tileButtons = new List<TileButton>();

	private List<List<TileType>> tileGroups = new List<List<TileType>>();
	private int tileGroupIndex = -1;

	private TileType activeTile = TileType.NONE;

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
			TileType.SOLID2,
			TileType.SEMISOLID,
			TileType.LADDER,
			TileType.BUSH,		// Remove
			TileType.CLOUD,		// Remove
			TileType.MOUNTAIN,	// Remove
			TileType.FLOWER,	// Remove
			TileType.CRATE,		// Remove
			TileType.SWEETS,	// Remove
			TileType.UFO,		// Remove
			TileType.SPAWNER,	// Remove
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

		Assert.AreEqual(selectedColors.Length, 12);

		// Create an array of 12 buttons on the Palette.
		for(int i = 0; i < 12; ++i)
		{
			TileButton newButton = Instantiate(tileButtonPrefab, tileButtonRoot);
			tileButtons.Add(newButton);
			newButton.palette = this;
			newButton.selectedColor = selectedColors[i];
		}
	}

	private void Start()
	{
		SwitchTileGroup(1);
	}

	public void SwitchTileGroup(int amount)
	{
		tileGroupIndex = (tileGroupIndex + amount) % tileGroups.Count;
		if(tileGroupIndex < 0) tileGroupIndex = tileGroups.Count - 1;

		int i = 0;
		foreach(var tileButton in tileButtons)
			tileButton.SetTileType(tileGroups[tileGroupIndex][i++]);

		SwitchActiveTile(tileGroups[tileGroupIndex][0]);
	}

	public void SwitchActiveTile(TileType tileType)
	{
		if(activeTile != tileType)
		{
			activeTile = tileType;

			OnTileChanged(new TileChangedEventArgs(tileType));

			foreach(TileButton button in tileButtons)
				button.SetIsCurrent(button.tileType == tileType);
		}
	}
}

public class TileChangedEventArgs : EventArgs
{
	public readonly TileType tileType;

	public TileChangedEventArgs(TileType tileType) : base()
	{
		this.tileType = tileType;
	}
}
