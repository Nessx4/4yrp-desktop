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

    public bool capturing = false;
	private new SpriteRenderer renderer;

    [SerializeField]
    private LayerMask enemyMask;

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
            capturing = true;
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

    bool condition1 = true;
    bool condition2 = false;
    public RaycastHit2D hit;
    public void Update()
    {
        if (!capturing)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.up, 0.01f, enemyMask);
            if (hit.collider != null && condition1)
            {
                Debug.Log(hit.collider.gameObject.transform.parent.gameObject.name);
                Prototype.proto.AddToWriteQueue(hit.collider.gameObject.transform.parent.gameObject.name);
                condition1 = false;
                condition2 = true;
            }

            if (hit.collider == null && condition2)
            {
                condition1 = true;
                Debug.Log("off");
                Prototype.proto.AddToWriteQueue("capture_off");
                condition2 = false;
            }
        }

    }

    public void Invisible()
    {
        renderer.enabled = false;
    }

    public void Visible()
    {
        renderer.enabled = true;
    }

}

public enum PointerType
{
	DESKTOP, MOBILE
}
