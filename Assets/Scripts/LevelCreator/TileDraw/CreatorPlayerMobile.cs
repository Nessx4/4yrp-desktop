using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CreatorPlayerMobile : CreatorPlayer 
{
    protected override void UpdatePreviewPos()
    {
        throw new System.NotImplementedException();
    }

	// While still holding down the placement button, continually place or
    // remove tiles.
    protected override IEnumerator PencilDraw()
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        CreatorTile newTilePre = activeTile.creatorPrefab;

        List<TilePosition> tilePositions = new List<TilePosition>();
        Stack<TileOperation> operations = new Stack<TileOperation>();

        while (!stopDrawing)
        {
            // Do not place tiles when mouse is on top of the UI elements.
            //if (!EventSystem.current.IsPointerOverGameObject())
            //{
                //Vector3 mousePos = new Vector3(Input.mousePosition.x,
                    //Input.mousePosition.y, 10.0f);
                Vector3 pos = PointerController.instance.GetPointerPos(0);

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

                if (!hasPlacedHere)
                {
                    pos.x = tp.x;
                    pos.y = tp.y;
                    pos.z = 0.0f;

                    tilePositions.Add(tp);

                    RaycastHit2D hitObj = Physics2D.Raycast(pos, Vector3.up, 0.25f, mask);
                    CreatorTile existingTile = null;

                    if (hitObj.transform != null)
                        existingTile = hitObj.transform.GetComponent<CreatorTile>();

                    // Don't place tiles if the result would be the same.
                    bool sameTile = (existingTile != null && existingTile.GetTilePrefab() == newTilePre);
                    // Don't replace air with air.
                    bool bothAir = (existingTile == null && newTilePre == null);

                    if (!sameTile && !bothAir)
                    {
                        if(newTilePre != null)
							operations.Push(new TileOperation(newTilePre, existingTile, pos));
				}
             
                }
            //}

            yield return wait;
        }

		// Add the drawn tiles to the undo history.
		if (operations.Count > 0)
			AddUndoHistory(operations);
	}

	protected override IEnumerator Erase()
	{
        HashSet<Vector2> tilePositions = new HashSet<Vector2>();
        Stack<TileOperation> operations = new Stack<TileOperation>();

        stopDrawing = false;

        while(!stopDrawing)
        {
            //Vector2 tp = RoundVectorToInt(MouseToWorldPos());

            /// GET POINTER POSITION
            Vector2 tp = RoundVectorToInt(PointerController.instance.GetPointerPos(0));

            // Do not re-erase here if already erased here this operation.
            if(!tilePositions.Contains(tp))
            {
                tilePositions.Add(tp);
                TryPlaceTile(ref operations, null, tp);
            }

            yield return null;
        }

        // Add the drawn tiles to the undo history.
        if (operations.Count > 0)
            AddUndoHistory(operations);
	}

    protected override IEnumerator Grab()
    {
        yield return null;

        throw new System.NotImplementedException();
    }

	protected override void FloodFill()
	{
		throw new System.NotImplementedException();
	}

	protected override IEnumerator DrawRect(bool filled)
	{
		throw new System.NotImplementedException();
	}

	public override void ClearAll()
	{
		throw new System.NotImplementedException();
	}
}
