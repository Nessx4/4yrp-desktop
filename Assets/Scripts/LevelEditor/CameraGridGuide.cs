using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraGridGuide : MonoBehaviour 
{
	[SerializeField]
	private Shader gridShader;
	private Material mat;

	private new Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
		mat = new Material(gridShader);
		mat.SetTexture("_GridTex", 
			Resources.Load<Texture>("UI/Editor/tx_GridGuide"));
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		// Work out the camera position and shift due to camera size.
		float x = transform.position.x - 0.5f - 
			(camera.aspect * camera.orthographicSize);
		float y = transform.position.y - 0.5f - camera.orthographicSize;
		Vector2 offset = new Vector2(x % 1.0f, y % 1.0f);
		Vector2 scale = new Vector2(camera.orthographicSize * camera.aspect, 
			camera.orthographicSize);

		mat.SetVector("_OffsetAndScale", 
			new Vector4(offset.x, offset.y, scale.x, scale.y));

		Graphics.Blit(src, dst, mat);
	}
}
