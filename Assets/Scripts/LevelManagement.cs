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

public class LevelManagement : MonoBehaviour
{
	public static LevelManagement instance;

	static LevelManagement()
	{
		instance = (new GameObject()).AddComponent<LevelManagement>();
	}

	public long id = -1;

	public void Save(string levelName)
	{
		var conn = LocalConnect(Application.persistentDataPath + "/local.db");

		if(!TableExists(conn, "levels"))
		{
			try 	{ CreateTable(conn); }
			catch 	(SqliteSyntaxException e) {}
		}

		TileType[,] tileTypes = LevelEditor.instance.editorGrid.GetTileTypes();

		BinaryFormatter bf = new BinaryFormatter();

		Directory.CreateDirectory(Application.persistentDataPath + 
			"/levels/local/");

		// If ID is -1.
		if(id == -1)
		{
			string levelPath = "/levels/local/" + GetNextLevelID(conn) + ".dat";

			FileStream file = File.Create(Application.persistentDataPath + 
				levelPath);
			bf.Serialize(file, tileTypes);
			file.Close();

			// Create the level and keep track of the ID.
			id = InsertLevel("User", levelName, 
				"path_to_screenshot", levelPath, conn);
		}
		else
		{

		}

		//StartCoroutine(UploadLevel(""));

		// If ID is not -1.
		// Do an update?
	}

	public TileType[,] Load(int id)
	{
		var conn = LocalConnect(Application.persistentDataPath + "/local.db");

		if(!TableExists(conn, "levels"))
		{
			try 	{ CreateTable(conn); }
			catch 	(SqliteSyntaxException e) { return null; }
		}

		List<string> data = SelectLevel(id, conn);

		BinaryFormatter bf = new BinaryFormatter();

		string levelPath = Application.persistentDataPath + data[4];
		FileStream file = File.Open(levelPath, FileMode.Open);
		TileType[,] tileTypes = (TileType[,])bf.Deserialize(file);
		file.Close();

		return tileTypes;

		//return null;
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
	private bool TableExists(IDbConnection conn, string tableName)
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
								"datapath TEXT NOT NULL);";

		cmd.CommandText = queryString;
		cmd.ExecuteNonQuery();
	}

	private long InsertLevel(string name, string desc, 
		string snapshotPath, string dataPath, IDbConnection conn)
	{
		IDbCommand cmd = conn.CreateCommand();

		string queryString =	"INSERT INTO " +
								"levels(name, desc, snapshot, datapath)" +
								"VALUES (" +
								"\"" + name + "\", " +
								"\"" + desc + "\", " +
								"\"" + snapshotPath + "\", " +
								"\"" + dataPath + "\");";

		cmd.CommandText = queryString;
		cmd.ExecuteNonQuery();

		cmd.CommandText = "SELECT last_insert_rowid()";

		return (long)cmd.ExecuteScalar();
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
			//Debug.Log(reader[0]);
			//Debug.Log(reader[1]);
			//Debug.Log(reader[2]);
			//Debug.Log(reader[3]);
			//Debug.Log(reader[4]);
		}

		return data;
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

	/*
	private static void ExecuteQuery(string queryString, 
		IDbConnection connection)
	{
		IDbCommand command = connection.CreateCommand();
		command.CommandText = queryString;

		command.ExecuteNonQuery();
	}

	private static void ExecuteReadQuery(string queryString, 
		IDbConnection connection)
	{
		IDbCommand command = connection.CreateCommand();
		command.CommandText = queryString;
		IDataReader reader = command.ExecuteReader();
	}
	*/
}

[System.Serializable]
public struct LevelData
{
	public TileType[,] tileTypes;

	public LevelData(TileType[,] tileTypes)
	{
		this.tileTypes = tileTypes;
	}
}
