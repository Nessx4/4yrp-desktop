using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class OptionsMenu : MonoBehaviour
{
	[SerializeField]
	private MenuRoot root;

	[SerializeField]
	private ButtonRemap[] remapButtons;

	private CanvasGroup grp;

	// Is any button remap object waiting for an input?
	private ButtonRemap activeRemap;

	public static OptionsMenu menu;

	private void Awake()
	{
		menu = this;

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

	public bool SetActiveRemap(ButtonRemap remap)
	{
		if(activeRemap == null)
		{
			foreach (ButtonRemap rb in remapButtons)
				rb.btn.enabled = false;

			activeRemap = remap;
			return true;
		}

		return false;
	}

	public void RemoveActiveRemap()
	{
		activeRemap = null;

		foreach (ButtonRemap rb in remapButtons)
			rb.btn.enabled = true;
	}

	private void OnEnable()
	{
		Load();
	}

	public void Close()
	{
		Save();

		if(activeRemap == null)
			root.ChangeToStart();
	}

	private void Save()
	{
		OptionsData data = new OptionsData();

		foreach (ButtonRemap rb in remapButtons)
			data.buttons.Add(new RemapData(rb.buttonName.text, rb.GetCode()));

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/options.dat");
		bf.Serialize(file, data);
		file.Close();
	}

	private void Load()
	{
		OptionsData data = null;

		if(File.Exists(Application.persistentDataPath + "/options.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + 
				"/options.dat", FileMode.Open);

			data = (OptionsData)bf.Deserialize(file);
			file.Close();

			if(data != null)
			{
				int i = 0;

				foreach(RemapData rd in data.buttons)
					remapButtons[i++].SetValues(rd.buttonName, rd.activeKey);
			}
		}
	}
}

[System.Serializable]
public struct RemapData
{
	public string buttonName;
	public KeyCode activeKey;

	public RemapData(string buttonName, KeyCode activeKey)
	{
		this.buttonName = buttonName;
		this.activeKey = activeKey;
	}
}

[System.Serializable]
public class OptionsData
{
	public List<RemapData> buttons;

	public OptionsData()
	{
		buttons = new List<RemapData>();
		Debug.Log(buttons);
	}

	public OptionsData(List<RemapData> buttons)
	{
		this.buttons = buttons;
	}
}
