using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;

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

	private const int startPort = 9000;

	private ConcurrentQueue<string> receiveQueue;
	private ConcurrentQueue<string> sendQueue;

	private bool shouldQuit = false;

	[SerializeField]
	private Transform mobileCursor;

	[SerializeField]
	private Camera mobileCamera;

	public static MobileConnection instance { get; private set; }

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			SetupThreads();
		}
		else Destroy(gameObject);
	}

	private void SetupThreads()
	{
		receiveQueue = new ConcurrentQueue<string>();
		sendQueue = new ConcurrentQueue<string>();

		IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

		for (int i = 0; i < localIPs.Length; ++i)
			Debug.Log(localIPs[i]);

		listenThread = new Thread(new ThreadStart(Connect));
		listenThread.Start();
	}

	private void Connect()
	{
		listener = new TcpListener(IPAddress.Parse("0.0.0.0"), startPort);
		listener.Start();

		Debug.Log("Awaiting connections on port " + startPort + ".");
		soc = listener.AcceptSocket();

		Debug.Log("Received connection on port " + startPort + ".");

		Listen();
	}

	// Listen for messages from the mobile device.
	private void Listen()
	{
		try
		{
			NetworkStream stream = new NetworkStream(soc);
			StreamReader reader = new StreamReader(stream);

			writer = new StreamWriter(stream);
			writer.AutoFlush = true;
			writeThread = new Thread(new ThreadStart(Write));
			writeThread.Start();

			// Enqueue all messages pending being received from mobile.
			string message;
			while((message = reader.ReadLine()) != null)
			{
				Debug.Log("Attempting read");
				if(!string.IsNullOrEmpty(message))
				{
					//Debug.Log(message);
					receiveQueue.Enqueue(message);
				}

				Debug.Log("Hi?");
			}
		}
		catch(IOException e)
		{
			Debug.LogError("Error while listening for messages.\n" + e);
		}

		Debug.Log("Joining listen thread?");

		//listenThread.Join();
	}

	// Write messages to the mobile device.
	private void Write()
	{
		string message;
		while(!shouldQuit)
		{
			while(sendQueue.TryDequeue(out message))
			{
				writer.WriteLine(message);
			}
		}

		writeThread.Join();
	}

	// Read all pending messages that have been received.
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
			sendQueue.Enqueue("runtime");

		string message;
		while(receiveQueue.TryDequeue(out message))
		{
			switch(message)
			{
				case "action_start":
					break;
				case "action_end":
					break;
				case "capture":
					break;
				case "leave_ufo":
					break;
				case "leave_slime":
					break;
				case "leave_spike":
					break;
				case "left":
					break;
				case "left2":
					break;
				case "right":
					break;
				case "right2":
					break;
				case "stop":
					break;
				case "fire":
					break;
				case "slime_left":
					break;
				case "slime_right":
					break;
				case "shake":
					break;
				case "spike":
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

					Debug.Log("Movement vector: " + floats[0] + ", " + floats[1]);

					mobileCursor.Translate(new Vector2(-floats[0], floats[1]) * 5.0f);

					break;
			}
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
