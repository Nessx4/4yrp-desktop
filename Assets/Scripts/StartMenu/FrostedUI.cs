using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FrostedUI : MonoBehaviour
{
	private Color mainCol;
	private float blur;

	private Material mat;
	private Image img;

	private void Start()
	{
		// Must create a copy of the material because Unity does not copy
		// materials from Image components.
		img = GetComponent<Image>();

		mat = new Material(img.material);
		img.material = mat;
	}

	public IEnumerator ChangeColour(float targetAlpha, float targetBlur)
	{
		// Set whether the image blocks raycasts to lower layers.
		img.raycastTarget = !Mathf.Approximately(targetAlpha, 0.0f);

		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		float startAlpha = mainCol.a;
		float startBlur = blur;

		for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime * 8.0f)
		{
			SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, x));
			SetBlur(Mathf.Lerp(startBlur, targetBlur, x));
			
			yield return wait;
		}

		SetAlpha(targetAlpha);
		SetBlur(targetBlur);
	}

	private void SetAlpha(float alpha)
	{
		Color col = mainCol;
		col.a = alpha;
		mainCol = col;

		mat.SetColor("_Color", mainCol);
	}

	private void SetBlur(float blur)
	{
		this.blur = blur;

		mat.SetFloat("_Radius", blur);
	}
}
