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

using UnityEngine;

public static class LevelManagement 
{
	public static long id = -1;

	public static void Save()
	{
		var conn = LocalConnect(Application.persistentDataPath + "/local.db");

		if(!TableExists(conn, "levels"))
		{
			try
			{
				Debug.Log("Creating, or at least TRYING to");
				CreateTable(conn);
			}
			catch(SqliteSyntaxException e)
			{

			}
		}

		// If ID is -1.
		long id = InsertLevel("Fred", "This is mah level", "path_to_screenshot",
			"path_to_data", conn);

		SelectLevel(3, conn);

		// If ID is not -1.
		// Do an update?
	}

	public static void Load()
	{

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
	private static IDbConnection LocalConnect(string dbPath)
	{
		string connString = "URI=file:" + dbPath + ",version=3";

		IDbConnection conn = new SqliteConnection(connString);
		conn.Open();

		return conn;
	}

	// Open a connection to a remote database.
	private static IDbConnection RemoteConnect(string dbPath)
	{
		return null;
	}

	// Returns true if the requested table exists.
	private static bool TableExists(IDbConnection conn, string tableName)
	{
		IDbCommand cmd = conn.CreateCommand();

		string queryString =	"SELECT name " +
								"FROM sqlite_master " +
								"WHERE type=\"table\" AND name=\"" + 
								tableName + "\";";

		cmd.CommandText = queryString;

		return ((string)cmd.ExecuteScalar() == tableName);
	}

	private static void CreateTable(IDbConnection conn)
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

	private static long InsertLevel(string name, string desc, 
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

	private static void SelectLevel(int id, IDbConnection conn)
	{
		IDbCommand cmd = conn.CreateCommand();

		string queryString = 	"SELECT * " +
								"FROM levels " +
								"WHERE id=" + id + ";";

		cmd.CommandText = queryString;

		IDataReader reader = cmd.ExecuteReader();

		while(reader.Read())
		{
			Debug.Log(reader[0]);
			Debug.Log(reader[1]);
			Debug.Log(reader[2]);
			Debug.Log(reader[3]);
			Debug.Log(reader[4]);
		}
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
