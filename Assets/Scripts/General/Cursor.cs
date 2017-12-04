/*	A cursor can belong to the desktop player or a mobile player. The desktop player
 *	has their own white cursor, while each mobile cursor has the same sprite with
 *	a different colour and a mobile icon below it.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cursor : MonoBehaviour
{
	[SerializeField]
	private Sprite desktopCursor;

	[SerializeField]
	private Sprite mobileCursor;

	private CursorType type;

	public Camera cam;
    public float speed = 2.5f;

	private new SpriteRenderer renderer;

	private void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	public void SetCursorType(CursorType type)
	{
		this.type = type;

		if (type == CursorType.DESKTOP)
		{
			renderer.sprite = desktopCursor;

		}
		else
		{
			renderer.sprite = mobileCursor;

			switch (type)
			{
				case CursorType.MOBILE01:
					renderer.color = Color.red;
					break;
				case CursorType.MOBILE02:
					renderer.color = Color.blue;
					break;
				case CursorType.MOBILE03:
					renderer.color = Color.yellow;
					break;
				case CursorType.MOBILE04:
					renderer.color = Color.green;
					break;
			}
		}
	}

	public void Move(Vector2 deltaMove)
	{
		
	}
	
	/*
	private void Update ()
	{
		
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
    }

    private void LateUpdate()
    {
        Vector3 pos = cam.WorldToScreenPoint(target.position);
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        target.position = cam.ScreenToWorldPoint(pos);
    }

    public void Move(float x, float y)
    {
        target.position += new Vector3(x*-1*speed, y*speed, 0);
    }
	*/

	public enum CursorType
	{
		DESKTOP, MOBILE01, MOBILE02, MOBILE03, MOBILE04
	}
}
