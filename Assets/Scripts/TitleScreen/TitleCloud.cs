using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TitleCloud : MonoBehaviour 
{
	[SerializeField]
	private List<Sprite> sprites;

	private float speed = 0.2f;

	private void Awake()
	{
		speed += Random.Range(-0.1f, 1.0f);
		GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
	}

	private void Update()
	{
		transform.Translate(-Time.deltaTime / 5.0f, 0.0f, 0.0f);
		if(transform.position.x <= -12.0f)
			transform.position = new Vector2(12.0f, transform.position.y);
	}
}
