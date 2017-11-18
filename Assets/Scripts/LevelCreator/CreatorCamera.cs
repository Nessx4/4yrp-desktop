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
		float x = cam.transform.position.x - 0.5f - cam.aspect / 2;
		float y = cam.transform.position.y - 0.5f;
		Vector2 offset = new Vector2(x - (int)x, y - (int)y);
		Vector2 scale = new Vector2(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2);

		mat.SetVector("_OffsetAndScale", new Vector4(offset.x, offset.y, scale.x, scale.y));

		Graphics.Blit(src, dst, mat);
	}
}