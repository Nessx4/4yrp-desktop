using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class LevelBrowser : MonoBehaviour
{
	// Prefab of a level preview button.
	[SerializeField]
	private LevelPreviewBox box;

	// The root Transform all level preview buttons are held under.
	[SerializeField]
	private Transform container;

	private void Start()
	{
		string[] filenames = Directory.GetFiles(Application.persistentDataPath + "/levels");
		List<LevelSaveData> levels = new List<LevelSaveData>();

		// Load all level data objects.
		foreach (string filename in filenames)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(filename, FileMode.Open);
			LevelSaveData data = (LevelSaveData)bf.Deserialize(file);
			file.Close();

			levels.Add(data);
		}

		// Sort the list of data backwards, so oldest are last in list.
		levels.Sort((l1, l2) => l2.timestamp.CompareTo(l1.timestamp));

		foreach(LevelSaveData level in levels)
		{
			LevelPreviewBox newBox = Instantiate(box, container);
			newBox.SetParameters(level.name, null, Random.Range(1, 1000), Random.Range(1, 11), level.timestamp);
		}
	}
}
