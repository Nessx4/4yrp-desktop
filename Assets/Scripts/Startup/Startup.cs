using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
	private void Start()
	{
		SetResolution();

		StartCoroutine(LoadNextScene());
	}

	// Load a resolution/fullscreen preference or choose a default one.
	private void SetResolution()
	{
		ResolutionSaveData data;
		if (ResolutionSaveData.Load(out data))
		{
			Screen.SetResolution(data.width, data.height, data.fullscreen);
		}
		else
		{
			Resolution[] resolutions = Screen.resolutions;

			Screen.SetResolution(resolutions[resolutions.Length - 1].width,
				resolutions[resolutions.Length - 1].height, true);
		}
	}

	private IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(2.0f);

		SceneManager.LoadScene("sc_StartMenu");
	}
}

[System.Serializable]
public struct ResolutionSaveData
{
	public int width, height;
	public bool fullscreen;

	public static bool Load(out ResolutionSaveData data)
	{
		data = new ResolutionSaveData();

		return false;
	}
}
