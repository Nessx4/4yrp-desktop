/*	When hovered over certain UI elements, the tooltip displays a short and
 *	helpful text description of the tool or button.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
	[SerializeField]
	private Image border;

	[SerializeField]
	private Text tooltipText;

	[SerializeField]
	private Canvas canvas;

	private Color borderColor;
	private Color textColor;

	private Coroutine change;

	public static Tooltip tooltip;

	private void Start()
	{
		tooltip = this;

		borderColor = border.color;
		textColor = tooltipText.color;

		// Set the tooltip to invisible on Start.
		border.color = new Color(borderColor.r, borderColor.g, borderColor.b, 0.0f);
		tooltipText.color = new Color(textColor.r, textColor.g, textColor.b, 0.0f);
	}

	// Move the tooltip to follow the mouse cursor.
	private void Update()
	{
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
		transform.position = canvas.transform.TransformPoint(pos);
	}

	// Start hovering over an element.
	public void StartTip(string tip)
	{
		if (change != null)
			StopCoroutine(change);

		tooltipText.text = tip;

		change = StartCoroutine(OpenTip());
	}

	// Stop hovering over an element.
	public void EndTip()
	{
		if (change != null)
			StopCoroutine(change);

		change = StartCoroutine(CloseTip());
	}

	// Change the alpha value of the tooltip to 1.0f.
	private IEnumerator OpenTip()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		border.color = new Color(borderColor.r, borderColor.g, borderColor.b, 0.0f);
		tooltipText.color = new Color(textColor.r, textColor.g, textColor.b, 0.0f);
		yield return new WaitForSeconds(0.5f);

		for(float i = 0.0f; i < 0.5f; i += Time.deltaTime * 2.0f)
		{
			Color newColor = border.color;
			newColor.a = Mathf.Lerp(0.0f, 1.0f, i);
			border.color = newColor;

			Color textColor = tooltipText.color;
			textColor.a = newColor.a;
			tooltipText.color = textColor;

			yield return wait;
		}

		border.color = borderColor;
		tooltipText.color = textColor;
	}

	// Change the alpha value of the tooltip to 0.0f.
	private IEnumerator CloseTip()
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();

		float startAlpha = border.color.a;

		for (float i = 0.0f; i < 0.25f; i += Time.deltaTime * 4.0f)
		{
			Color newColor = border.color;
			newColor.a = Mathf.Lerp(startAlpha, 0.0f, i);
			border.color = newColor;

			Color textColor = tooltipText.color;
			textColor.a = newColor.a;
			tooltipText.color = textColor;

			yield return wait;
		}

		border.color = new Color(borderColor.r, borderColor.g, borderColor.b, 0.0f);
		tooltipText.color = new Color(textColor.r, textColor.g, textColor.b, 0.0f);
	}
}
