using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public interface IControllable
{
	void Control();
	void Move(Vector2 moveAmount);
	void Action();
	void Release();
}
