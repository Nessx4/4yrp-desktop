using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

[SelectionBase]
public class PlayTile : MonoBehaviour
{
	[SerializeField]
	private List<Sprite> sprites;

	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private void Start()
	{
		Assert.IsTrue(sprites.Count > 0);

		Sprite chosenSprite = sprites[Random.Range(0, sprites.Count)];

		spriteRenderer.sprite = chosenSprite;
	}
}
