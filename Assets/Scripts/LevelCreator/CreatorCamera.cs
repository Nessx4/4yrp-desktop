using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CreatorCamera : MonoBehaviour
{
	[SerializeField]
	private Material mat;

	private Camera cam;

	private void Start()
	{
		cam = GetComponent<Camera>();
	}

	private void Update()
	{
		transform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Time.deltaTime);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		float x = (cam.transform.position.x + 0.5f) % 1;
		float y = (cam.transform.position.y + 0.5f) % 1;
		Vector2 offset = new Vector2(x, y);
		Vector2 scale = new Vector2(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2);

		mat.SetVector("_OffsetAndScale", new Vector4(offset.x, offset.y, scale.x, scale.y));

		Graphics.Blit(src, dst, mat);
	}
}