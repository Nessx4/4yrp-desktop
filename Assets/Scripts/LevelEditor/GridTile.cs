/*	GridTile represents data about a visual tile that has been placed inside
 *	the EditorGrid.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

public class GridTile : MonoBehaviour
{
	[SerializeField]
	private List<Sprite> sprites;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public TileType tileType { get; set; }
	public GridPosition position { get; set; }
	private bool dead = false;

	private void Start()
	{
		Assert.IsTrue(sprites.Count > 0);

		Sprite chosenSprite = sprites[Random.Range(0, sprites.Count)];

		Vector2 spriteDeltaPos = new Vector2();
		spriteDeltaPos.x = spriteDeltaPos.y = 1024 / chosenSprite.pixelsPerUnit;
		spriteRenderer.transform.localPosition = spriteDeltaPos / 2;

		spriteRenderer.sprite = chosenSprite;
	}

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
		rb.AddForce(new Vector2(Random.Range(-50.0f, 50.0f), 
			Random.Range(-50.0f, 50.0f)));
		rb.gravityScale = 5.0f;
	}
}
