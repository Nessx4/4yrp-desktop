using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour 
{
	[SerializeField] 
	private Image img;

	[SerializeField]
	private Text txt;

	private Vector2 anchoredPosition;
	private Coroutine shakeRoutine;

	private new RectTransform transform;

	public static WarningMessage instance { get; private set; }

	private void Awake()
	{
		instance = this;
		
		transform = GetComponent<RectTransform>();
		anchoredPosition = transform.anchoredPosition;

		SetMessage(false, "");
	}

	public void SetMessage(bool show, string message)
	{
		img.gameObject.SetActive(show);

		txt.text = show ? message : "";

		if(show)
		{
			if(shakeRoutine != null)
				StopCoroutine(shakeRoutine);

			StartCoroutine(Shake());
		}
	}

	public void Hide()
	{
		img.gameObject.SetActive(false);
		txt.text = "";
	}

	private IEnumerator Shake()
	{
		throw new System.NotImplementedException();
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		transform.anchoredPosition = anchoredPosition;
		Vector2 offset = Vector2.zero;

		for(float i = 0; i < 0.25f; i += Time.deltaTime)
		{
			offset = new Vector2(0.0f, Mathf.Sin(i * 64.0f) * 10.0f);

			transform.anchoredPosition = anchoredPosition + offset;
			yield return wait;
		}

		transform.anchoredPosition = anchoredPosition;
	}
}
