using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ToolbarButton : MonoBehaviour 
{
	protected Button button;

	public Toolbar toolbar { protected get; set; }
	private bool canPress = true;

	protected virtual void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Start()
	{
		// Set the button to have a slight angle.
		//GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-5, 5)));
	}

	public void Press()
	{
		if(canPress)
			DoAction();
	}

	public void SetInteractible(bool canPress)
	{
		this.canPress = canPress;
		button.image.color = canPress ? Color.white : Color.grey;
	}

	public virtual void SetActiveTool(ToolType toolType)
	{
		return;
	}

	protected abstract void DoAction();
}
