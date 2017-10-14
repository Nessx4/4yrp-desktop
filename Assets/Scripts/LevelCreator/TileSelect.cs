using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect : MonoBehaviour
{
	[SerializeField]
	private GameObject linkedItem;

	public void OnPointerEnter()
	{
		Debug.Log("Tooltip: " + linkedItem.name);
	}
}