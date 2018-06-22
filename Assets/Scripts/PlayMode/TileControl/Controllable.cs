using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class Controllable : MonoBehaviour
{
	public abstract void Control();
	public abstract void Move(Vector2 moveAmount);
	public abstract void Action();
	public abstract void Release();
	public abstract string GetType();
}
