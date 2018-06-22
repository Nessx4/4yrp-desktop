using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;
using UnityEngine.SceneManagement;

using System.Threading;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Globalization;

public class MobileConnection : MonoBehaviour
{
	// Each mobile connection has a pointer unique to itself.
	[SerializeField]
	private Pointer pointer;

	public Camera mobileCamera { set; private get; }

	private Thread listenThread;
	private Thread writeThread;

	private TcpListener listener;
	private Socket soc;

	private ConcurrentQueue<string> receiveQueue;
	private ConcurrentQueue<string> sendQueue;

	private bool shouldQuit = false;
	private bool canSeeEnemy = false;

	[SerializeField]
	private Transform mobileCursor;

	[SerializeField]
	private MobileEditorPlayer editorPlayer;

	private Controllable controlledEnemy = null;

	public int id { private get; set; }

	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		editorPlayer.conn = this;
	}

	public void SetupThreads(int id, TcpListener listener, bool isNew)
	{
		this.id = id;
		this.listener = listener;
		receiveQueue = new ConcurrentQueue<string>();
		sendQueue = new ConcurrentQueue<string>();

		listenThread = new Thread(() => Connect(isNew));
		listenThread.Start();
	}

	private void Connect(bool isNew)
	{
		Debug.Log("Awaiting connection; id=" + id + ".");
		soc = listener.AcceptSocket();

		Debug.Log(soc);

		Debug.Log("Received connection; id=" + id + ".");

		receiveQueue.Enqueue("start_connection");
		if(isNew)
			receiveQueue.Enqueue("new_connection");

		NetworkStream stream = new NetworkStream(soc);

		StreamReader reader = new StreamReader(stream);
		StreamWriter writer = new StreamWriter(stream);

		writer.AutoFlush = true;

		writeThread = new Thread(() => Write(writer));
		writeThread.Start();

		Read(reader);
	}

	private void CheckForEnemies()
	{
		if(controlledEnemy == null)
		{
			Vector3 pos = PointerToWorldPos();
			pos.z = -10.0f;

			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, 0.1f);

			if(hit != null && hit.transform != null)
			{
				Controllable obj = hit.transform.gameObject.GetComponent<Controllable>();

				if(obj != null && !canSeeEnemy)
				{
					canSeeEnemy = true;

					switch(obj.GetType())
					{
						case "ufo":
							sendQueue.Enqueue("pobj_UFO");
							break;
						case "spikes":
							sendQueue.Enqueue("pobj_SpikeTrap");
							break;
					}
				}
				else if(obj == null && canSeeEnemy)
				{
					canSeeEnemy = false;
					sendQueue.Enqueue("capture_off");
				}
			}
		}
	}

	// Tell this connection which camera to render its pointer to.
	public void SetupCamera()
	{
		Color col;

		switch(id)
		{
			case 0:
				col = Color.red;
				break;
			case 1:
				col = Color.blue;
				break;
			case 2:
				col = Color.yellow;
				break;
			default:
				col = Color.green;
				break;
		}

		Debug.Log(mobileCamera);
		pointer.SetParams(mobileCamera, col);
	}

	// Listen for messages from the mobile device.
	private void Read(StreamReader reader)
	{
		try
		{
			// Enqueue all messages pending being received from mobile.
			string message;
			while((message = reader.ReadLine()) != null)
			{
				if(!string.IsNullOrEmpty(message))
				{
					receiveQueue.Enqueue(message);
				}
			}
		}
		catch(IOException e)
		{
			Debug.LogError("Error while listening for messages.\n" + e);
		}

		receiveQueue.Enqueue("close");
		Debug.Log("Read thread closing.");
	}

	// Write messages to the mobile device.
	private void Write(StreamWriter writer)
	{
		Debug.Log("The write thread was started");
		string message;
		while(!shouldQuit)
		{
			while(sendQueue.TryDequeue(out message))
			{
				writer.WriteLine(message);
			}
		}

		Debug.Log("Write thread closing.");
	}

	// Read all pending messages that have been received.
	private void Update()
	{
		if(editorPlayer.playerActive)
			CheckForEnemies();

		string message;
		while(receiveQueue.TryDequeue(out message))
		{
			Debug.Log(message);
			switch(message)
			{
				// Either a new or retried connection is started.
				case "start_connection":
					mobileCursor.gameObject.SetActive(true);
					editorPlayer.playerActive = true;
					break;

				// An explicitly brand new connection starts.
				case "new_connection":
					ConnectionManager.instance.CreateNewConnection();
					SetupCamera();
					break;

				case "action_start":
					editorPlayer.StartAction();
					break;
				case "action_end":
					editorPlayer.StopAction();
					break;
				case "capture":
					if(controlledEnemy == null)
					{
						// Search for enemy!

						// Raycast at the pointer position.
						// When a collider is encountered, check it is an enemy.
						// If it is, capture the enemy.
						Vector3 pos = PointerToWorldPos();
						pos.z = -10.0f;

						RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, 0.1f);

						Controllable obj = hit.transform.gameObject.GetComponent<Controllable>();

						if(obj != null)
						{
							controlledEnemy = obj;
							Debug.Log("Controlling enemy: " + controlledEnemy);
						}
					}
					break;
				case "leave_ufo":
				case "leave_slime":
				case "leave_spike":
					if(controlledEnemy != null)
					{
						Vector3 pos = controlledEnemy.transform.position;
						pointer.SetWorldPos(pos);

						controlledEnemy.Release();
						controlledEnemy = null;
					}
					break;
				case "left":
					if(controlledEnemy != null)
					{
						controlledEnemy.Move(new Vector2(-0.5f, 0.0f));
					}
					break;
				case "left2":
					if(controlledEnemy != null)
					{
						controlledEnemy.Move(new Vector2(-1.0f, 0.0f));
					}
					break;
				case "right":
					if(controlledEnemy != null)
					{
						controlledEnemy.Move(new Vector2(0.0f, 0.5f));
					}
					break;
				case "right2":
					if(controlledEnemy != null)
					{
						controlledEnemy.Move(new Vector2(0.0f, 1.0f));
					}
					break;
				case "stop":
					if(controlledEnemy != null)
					{
						controlledEnemy.Move(new Vector2(0.0f, 0.0f));
					}
					break;
				case "fire":
					if(controlledEnemy != null)
					{
						controlledEnemy.Action();
					}
					break;
				case "slime_left":
					if(controlledEnemy != null)
					{
						controlledEnemy.Move(new Vector2(-1.0f, 0.0f));
					}
					break;
				case "slime_right":
					if(controlledEnemy != null)
					{
						controlledEnemy.Move(new Vector2(0.0f, -1.0f));
					}
					break;
				case "shake":
					if(controlledEnemy != null)
					{
						controlledEnemy.Action();
					}
					break;
				case "spike":
					if(controlledEnemy != null)
					{
						controlledEnemy.Action();
					}
					break;
				case "undo":
					editorPlayer.Undo();
					break;
				case "redo":
					editorPlayer.Redo();
					break;
				case "pencil":
					editorPlayer.ChangeTool(ToolType.PENCIL);
					break;
				case "pencil_end":
					break;
				case "eraser":
					editorPlayer.ChangeTool(ToolType.ERASER);
					break;
				case "eraser_end":
					break;
				case "basic 0":
					editorPlayer.ChangeTile(TileType.SOLID);
					break;
				case "basic 1":
					editorPlayer.ChangeTile(TileType.SEMISOLID);
					break;
				case "basic 2":
					editorPlayer.ChangeTile(TileType.LADDER);
					break;
				case "basic 3":
					editorPlayer.ChangeTile(TileType.CRATE);
					break;
				case "bg 0":
					editorPlayer.ChangeTile(TileType.BUSH);
					break;
				case "bg 1":
					editorPlayer.ChangeTile(TileType.CLOUD);
					break;
				case "bg 2":
					editorPlayer.ChangeTile(TileType.FLOWER);
					break;
				case "bg 3":
					editorPlayer.ChangeTile(TileType.MOUNTAIN);
					break;
				case "tech 0":
					editorPlayer.ChangeTile(TileType.START_POINT);
					break;
				case "tech 1":
					editorPlayer.ChangeTile(TileType.SWEETS);
					break;
				case "tech 2":
					editorPlayer.ChangeTile(TileType.UFO);
					break;
				case "tech 3":
					editorPlayer.ChangeTile(TileType.SPIKES);
					break;
				case "close":
					RetryConnection();
					break;
				default:
					if (message.StartsWith("ufo_angle:"))
					{
						float angle = float.Parse(message.Split(':')[1], CultureInfo.InvariantCulture);
                        Debug.Log(angle);
                        if(Mathf.Abs(angle) > 5)
                        {
                            if (angle < 0)
                            {
                                controlledEnemy.Move(new Vector2((Mathf.Max(angle, -50.0f) / 25.0f), 0));
                            }
                            else if (angle > 0)
                            {
                                controlledEnemy.Move(new Vector2((Mathf.Min(angle, 50.0f) / 25.0f), 0));
                            }
                        }
					}
					else
					{
						string[] floatStrings = message.Split(',');
						float[] floats = new float[floatStrings.Length];

						for (int i = 0; i < floats.Length; ++i)
							floats[i] = float.Parse(floatStrings[i], CultureInfo.InvariantCulture);

						pointer.Move(new Vector2(-floats[0], floats[1]) * 5.0f);
					}
					break;
			}
		}
	}

	public Vector3 PointerToWorldPos()
	{
		return pointer.PointerToWorldPos();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("Scene loaded: " + scene.name);
		switch(scene.name)
		{
			case "sc_LevelEditor":
				sendQueue.Enqueue("editor");
				break;
			case "sc_PlayMode":
				sendQueue.Enqueue("runtime");
				break;
		}

		editorPlayer.enabled = scene.name == "sc_LevelEditor";
	}

	private void RetryConnection()
	{
		OnDestroy();

		shouldQuit = false;

		Debug.Log("Set up brand new connections HERE");
		SetupThreads(id, listener, false);
	}

	private void OnDestroy()
	{
		shouldQuit = true;
		editorPlayer.playerActive = false;

		if (soc != null)
			soc.Close();

		if(writeThread != null)
		{
			writeThread.Abort();
			writeThread.Join();
		}

		if(listenThread != null)
		{
			listenThread.Abort();
			listenThread.Join();
		}
	}
}
