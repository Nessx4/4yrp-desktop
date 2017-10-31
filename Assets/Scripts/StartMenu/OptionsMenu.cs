using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class OptionsMenu : MonoBehaviour
{
	[SerializeField]
	private MenuRoot root;

	private CanvasGroup grp;

	private void Awake()
	{
		grp = GetComponent<CanvasGroup>();
	}

	public IEnumerator ChangeTransparency(float targetTrans)
	{
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float startTrans = grp.alpha;

		for (float x = 0.0f; x <= 1.0f; x += Time.deltaTime * 8.0f)
		{
			float newTrans = Mathf.Lerp(startTrans, targetTrans, x * 8.0f);
			grp.alpha = newTrans;
			yield return wait;
		}

		grp.alpha = targetTrans;

		// If becoming visible, set active (and vice versa).
		gameObject.SetActive(Mathf.Approximately(targetTrans, 1.0f));
	}

	public void Close()
	{
		root.ChangeToStart();
	}
}
