using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TitleCamera : MonoBehaviour 
{
	private void Awake()
	{
		Manager.mainCamera = GetComponent<Camera>();
	}
}
