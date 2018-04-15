/*	GridTile represents data about a visual tile that has been placed inside
 *	
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GridTile : MonoBehaviour
{
	private int _x;
	private int _y;

	private bool dead = false;

	public int x { get { return _x; } private set { _x = value; } }
	public int y { get { return _y; } private set { _y = value; } }

	private void Update()
	{
		if (dead && transform.position.y < -5.0f)
			Destroy(gameObject);
	}

	// Add a Rigidbody and make the object spin downwards towards doom.
	public void Kill()
	{
		dead = true;
		Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
		rb.AddTorque(Random.Range(-100.0f, 100.0f));
		rb.AddForce(new Vector2(Random.Range(-25.0f, 25.0f), 
			Random.Range(-25.0f, 25.0f)));
		rb.gravityScale = 5.0f;
	}
}
