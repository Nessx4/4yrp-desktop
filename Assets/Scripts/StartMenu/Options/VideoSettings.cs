using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class VideoSettings : MonoBehaviour 
{
	[SerializeField]
	private ToggleButton textureQuality;

	[SerializeField]
	private ToggleButton shadowQuality;

	[SerializeField]
	private ToggleButton antialiasing;

	[SerializeField]
	private ToggleButton verticalSync;

	[SerializeField]
	private SliderValue brightness;

	[SerializeField]
	private SliderValue contrast;

	public VideoData Save()
	{
		VideoData data = new VideoData(textureQuality.GetIndex(), 
			shadowQuality.GetIndex(), antialiasing.GetIndex(), 
			verticalSync.GetIndex(), brightness.GetValue(), contrast.GetValue());

		return data;
	}

	public void Load(VideoData data)
	{
		textureQuality.SetIndex(data.texIndex);
		shadowQuality.SetIndex(data.shadowIndex);
		antialiasing.SetIndex(data.antialiasIndex);
		verticalSync.SetIndex(data.vSyncIndex);
		brightness.SetValue(data.brightness);
		contrast.SetValue(data.contrast);
	}
}

[System.Serializable]
public struct VideoData
{
	public int texIndex;
	public int shadowIndex;
	public int antialiasIndex;
	public int vSyncIndex;
	public float brightness;
	public float contrast;

	public VideoData(int texIndex, int shadowIndex, int antialiasIndex, 
		int vSyncIndex, float brightness, float contrast)
	{
		 this.texIndex = texIndex;
		 this.shadowIndex = shadowIndex;
		 this.antialiasIndex = antialiasIndex;
		 this.vSyncIndex = vSyncIndex;
		 this.brightness = brightness;
		 this.contrast = contrast;
	}
}