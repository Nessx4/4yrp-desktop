using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour 
{
	[SerializeField]
	private Slider slider;

	[SerializeField]
	private Text sliderValue;

	public void SetValue(float value)
	{
		slider.value = value;

		ValueChanged();
	}

	public float GetValue()
	{
		return slider.value;
	}

	public void ValueChanged()
	{
		sliderValue.text = ((int)(slider.value * 100.0f)).ToString();
	}
}
