/*	The level browser is a screen which allows players to pick an existing level
 *	to edit, or create a new one. It also allows for deleting levels.
 *
 *	This class defines the structure of the saving and loading system. There is
 *	one Level database, which contains a dictionary of filenames pointing to
 *	level names. The browser list is populated using this data. If the database
 *	file does not exist, then it is created here.
 */
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
		Load();
	}

	private void Load()
	{
		BinaryFormatter bf = new BinaryFormatter();
		string file_to_open = Application.persistentDataPath + "/levels/db.dat";

		// Load the file database.
		if(File.Exists(file_to_open))
		{
			FileStream file = File.Open(file_to_open, FileMode.Open);
			LevelDatabase db = (LevelDatabase)bf.Deserialize(file);
			file.Close();

			//List<string> filenames = new List<string>(db.Keys);

			Debug.Log("Loading files!");

			if(db.names == null)
				Debug.Log("Database is empty!");

			foreach(string filename in db.names.Keys)
			{
				file = File.Open(filename, FileMode.Open);
				LevelSaveData data = (LevelSaveData)bf.Deserialize(file);
				file.Close();

				LevelPreviewBox newBox = Instantiate(box, container);

				Texture2D preview = new Texture2D(800, 200, TextureFormat.RGB24, false);
				preview.LoadImage(data.previewImage);
				
				Sprite previewSprite = Sprite.Create(preview, new Rect(0.0f, 0.0f, preview.width, preview.height), new Vector2(0.5f, 0.5f), 100.0f);
				newBox.SetParameters(data.name, filename, previewSprite, Random.Range(1, 1000), Random.Range(1, 11), data.timestamp);
			}
		}
		else
		{
			// Create the file database if it does not exist.
			Directory.CreateDirectory(Application.persistentDataPath + "/levels/");
			FileStream file = File.Create(Application.persistentDataPath + "/levels/db.dat");

			LevelDatabase db = new LevelDatabase(null);
			bf.Serialize(file, db);
			file.Close();

			Load();
		}

		/*
		if(Directory.Exists(Application.persistentDataPath + "/levels"))
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

			// Create an entry in the browser list for each level.
			foreach (LevelSaveData level in levels)
			{
				LevelPreviewBox newBox = Instantiate(box, container);

				Texture2D preview = new Texture2D(800, 200, TextureFormat.RGB24, false);
				preview.LoadImage(level.previewImage);
				
				Sprite previewSprite = Sprite.Create(preview, new Rect(0.0f, 0.0f, preview.width, preview.height), new Vector2(0.5f, 0.5f), 100.0f);
				newBox.SetParameters(level.name, previewSprite, Random.Range(1, 1000), Random.Range(1, 11), level.timestamp);
			}
		}
		*/
	}
}
