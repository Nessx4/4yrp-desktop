/*	Provides functionality to take a screenshot of the level at a set size.
 *	This will be used by the saving script to save a snapshot of the level
 *	with the rest of the level data.
 */
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PreviewCamera : MonoBehaviour 
{
	private Camera screenshotCam;

	public static PreviewCamera cam { get; private set; }

	private void Start()
	{
		if(cam == null)
		{
			cam = this;

			screenshotCam = GetComponent<Camera>();
			screenshotCam.enabled = false;
		}
		else
			Destroy(gameObject);
	}

	// Position the snapshot camera relative to the main camera and take a
	// screenshot of the level.
	public byte[] TakeScreenshot(Camera mainCam)
	{
		// Create and set textures on the camera.
		RenderTexture activeTexture = RenderTexture.active;
		RenderTexture tempTexture = new RenderTexture(800, 800, 24);
		screenshotCam.targetTexture = tempTexture;
		screenshotCam.orthographicSize = mainCam.aspect * 7.5f;
		screenshotCam.aspect = 1.0f;

		Vector3 pos = mainCam.transform.position + new Vector3(0.0f,
			screenshotCam.orthographicSize - mainCam.orthographicSize, 0.0f);

		pos.z = -5.0f;

		// Move the preview camera where it needs to be.
		transform.position = mainCam.transform.position + new Vector3(0.0f,
			screenshotCam.orthographicSize - mainCam.orthographicSize, 0.0f);

		transform.position = pos;

		RenderTexture.active = screenshotCam.targetTexture;
		screenshotCam.Render();

		Texture2D previewImage = new Texture2D(tempTexture.width, 
			tempTexture.height, TextureFormat.RGB24, false);

		previewImage.ReadPixels(new Rect(0, 0, tempTexture.width, 
			tempTexture.height), 0, 0);
		
		previewImage.Apply();

		RenderTexture.active = activeTexture;
		screenshotCam.targetTexture = null;
		Destroy(tempTexture);

		byte[] bytes = previewImage.EncodeToPNG();
		return bytes;
	}
}
