using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class CreatorPlayerDesktop : CreatorPlayer 
{
	// Buttons can be greyed out due to tool selection or undo/redo stack size.
	[SerializeField]
	private ToolbarButton undoButton;

	[SerializeField]
	private ToolbarButton redoButton;

	[SerializeField]
	private ToolbarButton fillButton;

	[SerializeField]
	private ToolbarButton rectFilledButton;

	[SerializeField]
	private ToolbarButton rectHollowButton;

	private Camera mainCam;

	protected override void Start()
	{
		base.Start();

		// Camera.main is slow so cache it.
		mainCam = Camera.main;
	}

	protected override void Update()
	{
		base.Update();

		if(Input.GetMouseButtonDown(0))
			StartDraw();

		if(Input.GetMouseButtonUp(0))
			StopDraw();
	}

	// Position the preview block at mouse position.
	protected override void UpdatePreviewPos()
	{
		if(previewBlock != null)
		{
			Vector3 mousePos = new Vector3(Input.mousePosition.x, 
				Input.mousePosition.y, 10.0f);
			
			Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);

			pos.x = Mathf.RoundToInt(pos.x);
			pos.y = Mathf.RoundToInt(pos.y);
			pos.z = -5.0f;
			previewBlock.transform.position = pos;
		}
	}

	// Check if the Undo and Redo buttons need to be greyed out.
	public override void CheckHistory()
	{
		undoButton.SetVisible(undoStack.Count > 0);
		redoButton.SetVisible(redoStack.Count > 0);
	}

	// While still holding down the placement button, continually place or
	// remove tiles.
	protected override IEnumerator PencilDraw(CreatorTile newTilePre)
	{
		Debug.Log("HI");
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		List<TilePosition> tilePositions = new List<TilePosition>();

		List<TileOperation> operations = new List<TileOperation>();

		stopDrawing = false;

		while(!stopDrawing)
		{
			// Do not place tiles when mouse is on top of the UI elements.
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				Vector3 mousePos = new Vector3(Input.mousePosition.x, 
					Input.mousePosition.y, 10.0f);
				Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);

				TilePosition tp = new TilePosition(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

				bool hasPlacedHere = false;

				foreach (TilePosition tilePosition in tilePositions)
				{
					if (tilePosition == tp)
					{
						hasPlacedHere = true;
						continue;
					}
				}

				if(!hasPlacedHere)
				{
					pos.x = tp.x;
					pos.y = tp.y;
					pos.z = 0.0f;

					tilePositions.Add(tp);

					RaycastHit2D hitObj = Physics2D.Raycast(pos, Vector3.up, 0.25f, mask);
					CreatorTile existingTile = null;

					if(hitObj.transform != null)
						existingTile = hitObj.transform.GetComponent<CreatorTile>();

					// Don't place tiles if the result would be the same.
					bool sameTile = (existingTile != null && existingTile.GetTilePrefab() == newTilePre);
					// Don't replace air with air.
					bool bothAir = (existingTile == null && newTilePre == null);

					if(!sameTile && !bothAir)
						operations.Add(new TileOperation(newTilePre, existingTile, pos));
				}
			}

			yield return wait;
		}

		// Add the drawn tiles to the undo history.
		if (operations.Count > 0)
			AddUndoHistory(operations);

		stopDrawing = false;
	}

	protected override IEnumerator Erase()
	{
		yield return null;

		throw new System.NotImplementedException();
	}

	protected override void FloodFill()
	{
		throw new System.NotImplementedException();
	}

	protected override void DrawHollowRect()
	{
		throw new System.NotImplementedException();
	}

	protected override void DrawFullRect()
	{
		throw new System.NotImplementedException();
	}

	public override void ClearAll()
	{
		List<TileOperation> operations = new List<TileOperation>();

		List<CreatorTile> tiles = CreatorPlayerWrapper.Get().GetTiles();

		foreach (CreatorTile tile in tiles)
			operations.Add(new TileOperation(null, tile, tile.transform.position));

		Debug.LogError("ClearAll() must grey out the Clear button.");

		AddUndoHistory(operations);
	}

	// Grey out undo/redo buttons on desktop UI.
	public override void SetActiveTile(TileData tile)
	{
		base.SetActiveTile(tile);

		if(tile.IsUnitSize())
		{
			fillButton.SetVisible(true);
			rectFilledButton.SetVisible(true);
			rectHollowButton.SetVisible(true);
		}
		else
		{
			fillButton.SetVisible(false);
			rectFilledButton.SetVisible(false);
			rectHollowButton.SetVisible(false);
		}
	}
}
