/*	LevelManagement encapsulates all the functionality required for saving
 *	and loading a level. 
 *
 *	Additionally, it keeps track of the level that will next need to be loaded 
 *	across scenes, for example when a user selects a level from the Browser or 
 *	creates a new one (an ID of -1 is assigned by default).
 *
 *	Levels are stored locally using a SQL database on the user's hard disk.
 */

using System.Data;
//using Mono.Data.Sqlite;
using Mono.Data.SqliteClient;

using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.Networking;

using UnityEngine.TestTools;
using UnityEngine.Assertions;

public class LevelManagement : MonoBehaviour
{
	public static LevelManagement instance;

	static LevelManagement()
	{
		instance = (new GameObject()).AddComponent<LevelManagement>();
		DontDestroyOnLoad(instance.gameObject);
	}

	public long id = -1;

	public void Save(string levelName, string levelDesc)
	{
		var conn = LocalConnect(Application.persistentDataPath + "/local.db");

		if(!TableExists(conn, "levels"))
		{
			try 	{ CreateTable(conn); }
			catch 	(SqliteSyntaxException e) {}
		}

		TileType[,] tileTypes = LevelEditor.instance.editorGrid.GetTileTypes();
		ThemeType themeType = LevelEditor.instance.themebar.GetThemeType();

		BinaryFormatter bf = new BinaryFormatter();

		Directory.CreateDirectory(Application.persistentDataPath + 
			"/levels/local/");

		string levelPath = "/levels/local/" + id + ".dat";
		string snapPath = "/thumbnails/local/" + id + ".dat";

		if(id == -1)	// Level is new.
		{
			LevelData data = new LevelData(tileTypes, themeType);

			long id = GetNextLevelID(conn);

			levelPath = "/levels/local/" + id + ".dat";
			snapPath = "/thumbnails/local/" + id + ".dat";

			// Create the level and keep track of the ID.
			id = InsertLevel("User", levelName,
				snapPath, levelPath, themeType.ToString(), conn);
		}
		else 	// Level is being overwritten.
		{
			UpdateLevel(id, levelName, levelDesc, themeType.ToString(), conn);
		}

		Debug.Log("Overwrite the level");
		Debug.Log(levelPath);

		// Create the level data file.
		FileStream file = File.Open(Application.persistentDataPath + 
			levelPath, FileMode.Create);
		LevelData levelData = new LevelData(tileTypes, themeType);
		bf.Serialize(file, levelData);
		file.Close();
	}

	public LevelData Load(long id)
	{
		var conn = LocalConnect(Application.persistentDataPath + "/local.db");

		if(!TableExists(conn, "levels"))
		{
			try 	{ CreateTable(conn); }
			catch 	(SqliteSyntaxException e) { return new LevelData(); }
		}

		List<string> data = SelectLevel(id, conn);

		BinaryFormatter bf = new BinaryFormatter();

		string levelPath = Application.persistentDataPath + data[4];
		FileStream file = File.Open(levelPath, FileMode.Open);
		LevelData levelData = (LevelData)bf.Deserialize(file);
		file.Close();

		this.id = id;

		return levelData;
	}

	/*
	private static IDbConnection OpenDB(string dbFilepath)
	{
		//SqliteConnection.CreateFile(dbFilepath);

		string connectString = "URI=file:" + dbFilepath + ",version=3";

		IDbConnection connection = new SqliteConnection(connectString);
		//connection.CreateFile(dbName);
		connection.Open();

		return connection;
	}
	*/

	// Open a connection to a database file stored locally.
	private IDbConnection LocalConnect(string dbPath)
	{
		string connString = "URI=file:" + dbPath + ",version=3";

		IDbConnection conn = new SqliteConnection(connString);
		conn.Open();

		return conn;
	}

	// Upload a level to the remote Database Server.
	private IEnumerator UploadLevel(string dbPath)
	{
		var formData = new List<IMultipartFormSection>();

		formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));

		UnityWebRequest www = UnityWebRequest.Post("http://localhost:8069/upload.php", formData);

		yield return www.SendWebRequest();

		if(www.isNetworkError || www.isHttpError)
			Debug.LogError(www.error);
		else
			Debug.Log(www.downloadHandler.text);
	}

	// Download level data from the remote Database Server.
	private void DownloadLevel(string dbPath)
	{

	}

	// Returns true if the requested table exists.
	public bool TableExists(IDbConnection conn, string tableName)
	{
		IDbCommand cmd = conn.CreateCommand();

		string queryString =	"SELECT name " +
								"FROM sqlite_master " +
								"WHERE type=\"table\" AND name=\"" + 
								tableName + "\";";

		cmd.CommandText = queryString;

		return ((string)cmd.ExecuteScalar() == tableName);
	}

	private void CreateTable(IDbConnection conn)
	{
		IDbCommand cmd = conn.CreateCommand();

		string queryString = 	"CREATE TABLE levels (" +
								"id INTEGER PRIMARY KEY AUTOINCREMENT," +
								"name TEXT NOT NULL," +
								"desc TEXT NOT NULL," +
								"snapshot TEXT NOT NULL," +
								"datapath TEXT NOT NULL," +
								"theme INTEGER);";

		cmd.CommandText = queryString;
		cmd.ExecuteNonQuery();
	}

	private long InsertLevel(string name, string desc, 
		string snapshotPath, string dataPath, string themeType, 
		IDbConnection conn)
	{
		string queryString =	"INSERT INTO " +
								"levels(name, desc, snapshot, datapath, theme)" +
								"VALUES (" +
								"@param1, " +
								"@param2, " +
								"@param3, " +
								"@param4, " +
								"@param5 );";

		IDbCommand insertSql = conn.CreateCommand();
		insertSql.CommandText = queryString;
		insertSql.CommandType = CommandType.Text;

		insertSql.Parameters.Add(new SqliteParameter("@param1", SqlDbType.Text) { Value = name });
		insertSql.Parameters.Add(new SqliteParameter("@param2", SqlDbType.Text) { Value = desc });
		insertSql.Parameters.Add(new SqliteParameter("@param3", SqlDbType.Text) { Value = snapshotPath });
		insertSql.Parameters.Add(new SqliteParameter("@param4", SqlDbType.Text) { Value = dataPath });
		insertSql.Parameters.Add(new SqliteParameter("@param5", SqlDbType.Text) { Value = themeType });

		insertSql.ExecuteNonQuery();

		IDbCommand idSql = conn.CreateCommand();
		idSql.CommandText = "SELECT last_insert_rowid()";

		return (long)idSql.ExecuteScalar();
	}

	private List<string> SelectLevel(long id, IDbConnection conn)
	{
		IDbCommand cmd = conn.CreateCommand();

		string queryString = 	"SELECT * " +
								"FROM levels " +
								"WHERE id=" + id + ";";

		cmd.CommandText = queryString;

		IDataReader reader = cmd.ExecuteReader();

		List<string> data = new List<string>();

		while(reader.Read())
		{
			data.Add(reader[0].ToString());
			data.Add(reader[1].ToString());
			data.Add(reader[2].ToString());
			data.Add(reader[3].ToString());
			data.Add(reader[4].ToString());
		}

		return data;
	}

	private void UpdateLevel(long id, string name, string desc, 
		string themeType, IDbConnection conn)
	{
		string queryString = 	"UPDATE levels " +
								"SET name=@param1, " +
								"desc=@param2, " +
								"theme=@param3 " +
								"WHERE id=" + id + ";";

		IDbCommand updateSql = conn.CreateCommand();
		updateSql.CommandText = queryString;
		updateSql.CommandType = CommandType.Text;

		updateSql.Parameters.Add(new SqliteParameter("@param1", SqlDbType.Text) { Value = name });
		updateSql.Parameters.Add(new SqliteParameter("@param2", SqlDbType.Text) { Value = desc });
		updateSql.Parameters.Add(new SqliteParameter("@param3", SqlDbType.Text) { Value = themeType });

		updateSql.ExecuteReader();
	}

	private long GetNextLevelID(IDbConnection conn)
	{
		IDbCommand cmd = conn.CreateCommand();

		cmd.CommandText = "SELECT MAX(id) FROM levels;";

		IDataReader reader = cmd.ExecuteReader();

		if(reader.Read())
			return (reader[0] == null) ? 1 : (long)reader[0];

		return 0;
	}







	[UnityTest]
	public IEnumerator SampleSaveAndLoadDataFromDatabase()
	{
		Debug.Log("Running test");
		var conn = LocalConnect(Application.persistentDataPath + "/test.db");

		// Create a test level.
		TileType[,] tileTypes = new TileType[100, 100];
		tileTypes[5, 7] = TileType.SOLID;

		// Create the table.
		if(!TableExists(conn, "levels"))
		{
			try 	{ CreateTable(conn); }
			catch 	(SqliteSyntaxException e) 
			{
				// This assertion will always fail if the code reaches here.
				Assert.IsNull(e);
			}
		}

		Directory.CreateDirectory(Application.persistentDataPath + 
			"/levels/test/");

		// Ensure directory creation did not fail.
		Assert.IsTrue(Directory.Exists(Application.persistentDataPath + 
			"/levels/test/"));

		BinaryFormatter bf = new BinaryFormatter();

		string levelPath = "/levels/test/" + GetNextLevelID(conn) + ".dat";

		FileStream file = File.Create(Application.persistentDataPath + 
			levelPath);
		bf.Serialize(file, tileTypes);
		file.Close();

		// Assert that the level file exists.
		Assert.IsTrue(File.Exists(levelPath));

		// Create the level and keep track of the ID.
		id = InsertLevel("User", "Level Name", 
			"path_to_screenshot", levelPath, ThemeType.NORMAL.ToString(), conn);

		// Attempt to load the level from the database.
		List<string> data = SelectLevel(id, conn);

		levelPath = Application.persistentDataPath + data[4];
		file = File.Open(levelPath, FileMode.Open);
		//tileTypes = (TileType[,])bf.Deserialize(file);
		LevelData levelData = (LevelData)bf.Deserialize(file);
		file.Close();

		tileTypes = levelData.tileTypes;

		// Assert that the correct data was loaded.
		for(int x = 0; x < tileTypes.GetLength(0); ++x)
		{
			for(int y = 0; y < tileTypes.GetLength(1); ++y)
			{
				if(x == 5 && y == 7)
					Assert.AreEqual(tileTypes[x, y], TileType.SOLID);
				else
					Assert.AreEqual(tileTypes[x, y], TileType.NONE);
			}
		}

		// Delete all test files and assert they do not exist after deletion.
		File.Delete(levelPath);
		Assert.IsFalse(File.Exists(levelPath));

		Directory.Delete(Application.persistentDataPath + "/levels/test/");
		Assert.IsFalse(Directory.Exists(Application.persistentDataPath + "/levels/test/"));

		File.Delete(Application.persistentDataPath + "/test.db");
		Assert.IsFalse(File.Exists(Application.persistentDataPath + "/test.db"));

		yield return null;
	}
}

[System.Serializable]
public struct LevelData
{
	public readonly TileType[,] tileTypes;
	public readonly ThemeType themeType;

	/*
	public LevelData()
	{
		tileTypes = new TileType[100, 100];
		themeType = ThemeType.NORMAL;
	}
	*/

	public LevelData(TileType[,] tileTypes, ThemeType themeType)
	{
		this.tileTypes = tileTypes;
		this.themeType = themeType;
	}
}
