using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CreatorCamera : MonoBehaviour
{
	[SerializeField]
	private Material mat;

	private Vector2 lowerLeft = new Vector2(0, 0);
	private Vector2 upperRight = new Vector2(100, 100);

	private Camera cam;

	private void Start()
	{
		cam = GetComponent<Camera>();
	}

	private void Update()
	{
		Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		transform.Translate(move * 5.0f * Time.deltaTime);

		BoundPosition();
	}

	// Don't let the camera go too far off the edges of the editable world.
	private void BoundPosition()
	{
		Vector2 pos = transform.position;
		float camSize = cam.orthographicSize;

		pos.x = Mathf.Clamp(pos.x, lowerLeft.x + camSize, upperRight.x - camSize);
		pos.y = Mathf.Clamp(pos.y, lowerLeft.y + camSize, upperRight.y - camSize);

		transform.position = new Vector3(pos.x, pos.y, -10.0f);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		// Work out the x-position, shifted due to camera size.
		float x = cam.transform.position.x - 0.5f - (cam.aspect * cam.orthographicSize);
		float y = cam.transform.position.y - 0.5f - cam.orthographicSize;
		Vector2 offset = new Vector2(x % 1.0f, y % 1.0f);
		Vector2 scale = new Vector2(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2);

		mat.SetVector("_OffsetAndScale", new Vector4(offset.x, offset.y, scale.x, scale.y));

		Graphics.Blit(src, dst, mat);
	}
}