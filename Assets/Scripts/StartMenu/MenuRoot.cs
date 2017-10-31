using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRoot : MonoBehaviour
{
	[SerializeField]
	private StartMenu start;

	[SerializeField]
	private OptionsMenu options;

	private void Start()
	{
		options.gameObject.SetActive(false);
	}

	public void ChangeToStart()
	{
		start.Maximise();
		options.StartCoroutine(options.ChangeTransparency(0.0f));
	}

	public void ChangeToOptions()
	{
		start.Minimise();

		options.gameObject.SetActive(true);
		options.StartCoroutine(options.ChangeTransparency(1.0f));
	}
}
