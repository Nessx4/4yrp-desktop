using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour 
{
	[SerializeField]
	private SliderValue master;

	[SerializeField] 
	private SliderValue music;

	[SerializeField]
	private SliderValue sfx;

	public AudioData Save()
	{
		AudioData data = new AudioData(master.GetValue(), music.GetValue(), 
			sfx.GetValue());

		return data;
	}

	public void Load(AudioData data)
	{
		master.SetValue(data.master);
		music.SetValue(data.music);
		sfx.SetValue(data.sfx);
	}
}

[System.Serializable]
public struct AudioData
{
	public float master;
	public float music;
	public float sfx;

	public AudioData(float master, float music, float sfx)
	{
		this.master = master;
		this.music = music;
		this.sfx = sfx;
	}
}
