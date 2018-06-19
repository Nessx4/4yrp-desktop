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
	private Thread listenThread;
	private Thread writeThread;

	private TcpListener listener;
	private Socket soc;
	private StreamWriter writer;

	private ConcurrentQueue<string> receiveQueue;
	private ConcurrentQueue<string> sendQueue;

	private bool shouldQuit = false;

	[SerializeField]
	private Transform mobileCursor;

	[SerializeField]
	private Camera mobileCamera;

	private Controllable controlledEnemy = null;

	public int id { private get; set; }

	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		//SetupThreads();
	}

	public void SetupThreads(int id, TcpListener listener)
	{
		this.id = id;
		this.listener = listener;
		receiveQueue = new ConcurrentQueue<string>();
		sendQueue = new ConcurrentQueue<string>();

		IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

		for (int i = 0; i < localIPs.Length; ++i)
			Debug.Log(localIPs[i], gameObject);

		listenThread = new Thread(new ThreadStart(Connect));
		listenThread.Start();
	}

	private void Connect()
	{
		//Debug.Log("Awaiting connections on port " + startPort + ".");
		Debug.Log("Awaiting connections.");
		soc = listener.AcceptSocket();

		Debug.Log(soc);

		//Debug.Log("Received connection on port " + startPort + ".");
		Debug.Log("Received connections.");
		receiveQueue.Enqueue("start_connection");

		NetworkStream stream = new NetworkStream(soc);
		StreamReader reader = new StreamReader(stream);

		writer = new StreamWriter(stream);
		writer.AutoFlush = true;
		writeThread = new Thread(new ThreadStart(Write));
		writeThread.Start();

		Listen(reader);
	}

	// Listen for messages from the mobile device.
	private void Listen(StreamReader reader)
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
		listenThread.Join();
	}

	// Write messages to the mobile device.
	private void Write()
	{
		Debug.Log("The write thread was started");
		sendQueue.Enqueue("editor");
		string message;
		while(!shouldQuit)
		{
			while(sendQueue.TryDequeue(out message))
			{
				writer.WriteLine(message);
			}
		}

		Debug.Log("Stopped stuff");
		writeThread.Join();
	}

	// Read all pending messages that have been received.
	private void Update()
	{
		string message;
		while(receiveQueue.TryDequeue(out message))
		{
			Debug.Log(id);
			switch(message)
			{
				case "start_connection":
					ConnectionManager.instance.CreateNewConnection();
					mobileCursor.gameObject.SetActive(true);
					break;
				case "action_start":
					break;
				case "action_end":
					break;
				case "capture":
					if(controlledEnemy == null)
					{
						// Search for enemy!
					}
					break;
				case "leave_ufo":
				case "leave_slime":
				case "leave_spike":
					if(controlledEnemy != null)
					{
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
					break;
				case "redo":
					break;
				case "pencil":
					break;
				case "pencil_end":
					break;
				case "eraser":
					break;
				case "eraser_end":
					break;

				case "basic 0":
					break;
				case "basic 1":
					break;
				case "basic 2":
					break;
				case "basic 3":
					break;
				case "bg 0":
					break;
				case "bg 1":
					break;
				case "bg 2":
					break;
				case "bg 3":
					break;
				case "tech 0":
					break;
				case "tech 1":
					break;
				case "tech 2":
					break;
				case "tech 3":
					break;
				default:
					string[] floatStrings = message.Split(',');
					float[] floats = new float[floatStrings.Length];

					for (int i = 0; i < floats.Length; ++i)
						floats[i] = float.Parse(floatStrings[i], CultureInfo.InvariantCulture);

					//Debug.Log("Movement vector: " + floats[0] + ", " + floats[1]);

					mobileCursor.Translate(new Vector2(-floats[0], floats[1]) * 5.0f);

					break;
			}
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		switch(SceneManager.GetActiveScene().name)
		{
			case "sc_LevelEditor":
				sendQueue.Enqueue("editor");
				break;
			case "sc_PlayMode":
				sendQueue.Enqueue("runtime");
				break;
		}
	}

	private void OnDestroy()
	{
		shouldQuit = true;

		if (soc != null)
			soc.Close();

		writeThread.Abort();
		writeThread.Join();

		listenThread.Abort();
		listenThread.Join();
	}
}
