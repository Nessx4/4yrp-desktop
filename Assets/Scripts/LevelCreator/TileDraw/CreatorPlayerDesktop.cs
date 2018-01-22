using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class CreatorPlayerDesktop : CreatorPlayer 
{
	private Camera mainCam;

	protected override void Start()
	{
		base.Start();

		// Camera.main is slow so cache it.
		mainCam = Camera.main;
	}

	private void Update()
	{
		UpdatePreviewPos();

		if(Input.GetMouseButtonDown(0))
			StartDraw();

		if(Input.GetMouseButtonUp(0))
			StopDraw();
	}

	private Vector3 MouseToWorldPos()
	{
		Vector3 mousePos = new Vector3(Input.mousePosition.x, 
			Input.mousePosition.y, 10.0f);
		
		return mainCam.ScreenToWorldPoint(mousePos);
	}

	private Vector2 RoundVectorToInt(Vector2 vec)
	{
		return new Vector2(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y));
	}

	// Position the preview block at mouse position.
	protected override void UpdatePreviewPos()
	{
		if(previewBlock != null)
		{
			Vector3 pos = MouseToWorldPos();

			pos.x = Mathf.RoundToInt(pos.x);
			pos.y = Mathf.RoundToInt(pos.y);
			pos.z = -5.0f;
			previewBlock.transform.position = pos;
		}
	}

	// While still holding down the placement button, continually place or
	// remove tiles.
	protected override IEnumerator PencilDraw()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		HashSet<Vector2> tilePositions = new HashSet<Vector2>();
		HashSet<TileOperation> operations = new HashSet<TileOperation>();

		stopDrawing = false;

		while(!stopDrawing)
		{
			// Do not place tiles when mouse is on top of the UI elements.
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				Vector2 tp = RoundVectorToInt(MouseToWorldPos());

				if(!tilePositions.Contains(tp))
				{
					tilePositions.Add(tp);

					operations = TryPlaceTile(operations, 
						activeTile.creatorPrefab, tp);
				}
			}

			yield return wait;
		}

		// Add the drawn tiles to the undo history.
		if (operations.Count > 0)
			AddUndoHistory(operations);
	}

	protected override IEnumerator Erase()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		HashSet<Vector2> tilePositions = new HashSet<Vector2>();
		HashSet<TileOperation> operations = new HashSet<TileOperation>();

		stopDrawing = false;

		while(!stopDrawing)
		{
			// Do not place tiles when mouse is on top of the UI elements.
			if(!EventSystem.current.IsPointerOverGameObject())
			{
				Vector2 tp = RoundVectorToInt(MouseToWorldPos());

				// Do not re-erase here if already erased here this operation.
				if(!tilePositions.Contains(tp))
				{
					tilePositions.Add(tp);
					operations = TryPlaceTile(operations, null, tp);
				}
			}

			yield return wait;
		}

		// Add the drawn tiles to the undo history.
		if (operations.Count > 0)
			AddUndoHistory(operations);
	}

	protected override void FloodFill()
	{
		throw new System.NotImplementedException();
	}

	protected override IEnumerator DrawRect(bool filled)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		Vector2 startPos = RoundVectorToInt(MouseToWorldPos());
		Vector2 endPos = Vector2.zero;

		HashSet<CreatorTile> previews = new HashSet<CreatorTile>();

		stopDrawing = false;

		while(!stopDrawing)
		{
			//if(!EventSystem.current.IsPointerOverGameObject())
			//{
				foreach(CreatorTile preview in previews)
				{
					if(preview != null)
						Destroy(preview.gameObject);
				}
				previews.Clear();

				endPos = RoundVectorToInt(MouseToWorldPos());

				previews = RectHelper(startPos, endPos, filled, true);
			//}

			yield return wait;
		}

		foreach(CreatorTile preview in previews)
		{
			if(preview != null)
				Destroy(preview.gameObject);
		}
		previews.Clear();

		HashSet<CreatorTile> newTiles = RectHelper(startPos, endPos, filled, false);
	}

	public override void ClearAll()
	{
		HashSet<TileOperation> operations = new HashSet<TileOperation>();

		List<CreatorTile> tiles = CreatorPlayerWrapper.Get().GetTiles();

		foreach (CreatorTile tile in tiles)
			operations.Add(new TileOperation(null, tile, tile.transform.position));

		AddUndoHistory(operations);
	}
}
