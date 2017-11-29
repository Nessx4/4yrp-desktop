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

		foreach (string filename in filenames)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(filename, FileMode.Open);
			LevelSaveData data = (LevelSaveData)bf.Deserialize(file);
			file.Close();

			LevelPreviewBox newBox = Instantiate(box, container);
			newBox.SetParameters(data.name, null, Random.Range(1, 1000), Random.Range(1, 11));
		}
	}
}
