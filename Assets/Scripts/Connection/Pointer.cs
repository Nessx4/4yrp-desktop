using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pointer : MonoBehaviour 
{
	private Camera cam;

	private void Update()
	{
		Vector3 pos = cam.WorldToViewportPoint(transform.position);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		transform.position = cam.ViewportToWorldPoint(pos);
	}

	public void SetParams(Camera cam, Color col)
	{
		this.cam = cam;
		GetComponent<SpriteRenderer>().color = col;
	}

	public void Move(Vector2 amount)
	{
		transform.Translate(amount);
	}

	public Vector3 PointerToWorldPos()
	{
		return Manager.CursorToWorldPoint(cam.WorldToViewportPoint(transform.position));
	}

	public void SetWorldPos(Vector3 worldPos)
	{
		worldPos.z = 0.0f;
		transform.position = worldPos;
	}
}
