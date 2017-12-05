/*	A Pointer can belong to the desktop player or a mobile player. The desktop player
 *	has their own white Pointer, while each mobile Pointer has the same sprite with
 *	a different colour and a mobile icon below it.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Pointer : MonoBehaviour
{
	[SerializeField]
	private Sprite desktopPointer;

	[SerializeField]
	private Sprite mobilePointer;

	private PointerType type;

    private float speed = 15.0f;

	private new SpriteRenderer renderer;

	private void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	public void SetPointerType(PointerType type, int id)
	{
		this.type = type;

		if(renderer == null)
			renderer = GetComponent<SpriteRenderer>();

		if (type == PointerType.DESKTOP)
		{
			renderer.sprite = desktopPointer;
			Cursor.visible = false;
		}
		else
		{
			renderer.sprite = mobilePointer;

			switch (id)
			{
				case 0:
					renderer.color = Color.red;
					break;
				case 1:
					renderer.color = Color.blue;
					break;
				case 2:
					renderer.color = Color.yellow;
					break;
				case 3:
					renderer.color = Color.green;
					break;
			}
		}
	}

	public void Move(Vector2 move)
	{
		transform.position += new Vector3(-move.x, move.y, 0.0f) * speed;
	}

	public void SetPosition(Vector2 pos)
	{
		transform.position = new Vector3(pos.x, pos.y, 0.0f);
	}

	public void BoundPosition(Camera cam)
	{
		Vector3 pos = cam.WorldToScreenPoint(transform.position);
		pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
		pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
		transform.position = cam.ScreenToWorldPoint(pos);
	}
}

public enum PointerType
{
	DESKTOP, MOBILE
}
