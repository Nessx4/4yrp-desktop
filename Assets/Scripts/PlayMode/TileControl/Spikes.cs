using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Spikes : Controllable 
{
	private bool captured = false;
	private bool up = false;

	public override void Control()
	{
		captured = true;
	}

	public override void Move(Vector2 moveAmount)
	{
		
	}

	public override void Action()
	{
		Debug.Log("Do stuff");
	}

	public override void Release()
	{
		captured = false;
	}

	public override string GetType()
	{
		return "spikes";
	}
}
