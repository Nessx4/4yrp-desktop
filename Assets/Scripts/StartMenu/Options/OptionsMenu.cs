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

	// VBox containing remapping widgets.
	[SerializeField] 
	private RectTransform vBox;

	[SerializeField] 
	private ButtonRemap remapWidget;

	// Fields containing button mappings that can be modified.
	private List<ButtonRemap> remapButtons;

	[SerializeField]
	private List<RemapData> defaultMappings;

	private CanvasGroup grp;

	// Is any button remap object waiting for an input?
	private ButtonRemap activeRemap;

	public static OptionsMenu menu;

	private void Awake()
	{
		menu = this;

		grp = GetComponent<CanvasGroup>();
		remapButtons = new List<ButtonRemap>();

		Load();
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

		FindClashes();
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
		List<RemapData> remappings = new List<RemapData>();

		// Load mapping data from disk or use defaults.
		if(File.Exists(Application.persistentDataPath + "/options.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + 
				"/options.dat", FileMode.Open);

			data = (OptionsData)bf.Deserialize(file);
			file.Close();
		}
		else
		{
			data = new OptionsData(defaultMappings);
		}

		// Create the mapping widgets on the fly using the data loaded.
		if(data != null)
		{
			int height = 20;
			foreach(RemapData rd in data.buttons)
			{
				ButtonRemap rm = Instantiate(remapWidget);
				rm.transform.SetParent(vBox);
				rm.SetValues(rd.buttonName, rd.activeKey);

				height += 70;

				remapButtons.Add(rm);

				vBox.sizeDelta = new Vector2(800, height);
			}
		}
	}

	// Detect places where two keys have been assigned to the same action.
	private void FindClashes()
	{
		Dictionary<KeyCode, ButtonRemap> mappings = new Dictionary<KeyCode, ButtonRemap>();

		foreach(ButtonRemap rb in remapButtons)
		{
			KeyCode kc = rb.GetCode();

			if(mappings.ContainsKey(kc))
			{

			}
			else
				mappings.Add(kc, rb);
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
	}

	public OptionsData(List<RemapData> buttons)
	{
		this.buttons = buttons;
	}
}
