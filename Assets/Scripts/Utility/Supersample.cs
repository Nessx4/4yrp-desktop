using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Supersample : MonoBehaviour 
{
	[SerializeField] 
	private List<Camera> cameras;

	private const int factor = 2;

	private RenderTexture supersampleRT;

	private void Start()
	{
		supersampleRT = new RenderTexture(Screen.width * factor, Screen.height * factor, 24, RenderTextureFormat.ARGB32);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		foreach(var cam in cameras)
		{
			cam.targetTexture = supersampleRT;
			cam.Render();
			cam.targetTexture = null;
		}

		Graphics.Blit(supersampleRT, dst);
	}
}
