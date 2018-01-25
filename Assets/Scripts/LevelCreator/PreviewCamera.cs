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
		RenderTexture activeTexture = RenderTexture.active;
		RenderTexture tempTexture = new RenderTexture(800, 200, 24);
		screenshotCam.targetTexture = tempTexture;
		screenshotCam.orthographicSize = 2.5f * mainCam.aspect;
		screenshotCam.aspect = 4.0f;

		// Move the preview camera where it needs to be.
		transform.position = mainCam.transform.position + new Vector3(0.0f,
			screenshotCam.orthographicSize - mainCam.orthographicSize, 0.0f);

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
