using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;
using UnityEngine.UI;

using System.Threading;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Globalization;

public class Prototype : MonoBehaviour
{
	[SerializeField]
	private Player player;
    [SerializeField]
    private UFO ufo;
    [SerializeField]
    private Pointer pointer;

    // Network communication.
    private Thread listenThread;
    private TcpListener listener;
    private Socket soc;

	private int port = 9000;

	private ConcurrentQueue<string> commandQueue;

	// Begin a new thread to start listening on a socket.
    private void Start()
    {
    	PreSetup();
    }

    private void PreSetup()
    {
    	commandQueue = new ConcurrentQueue<string>();

        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

        for (int i = 0; i < localIPs.Length; i++)
            Debug.Log(localIPs[i]);

		listenThread = new Thread(new ThreadStart(Setup));
       	listenThread.Start();
    }

	// Accept a connection and begin to listen for messages.
    private void Setup()
    {
    	listener = new TcpListener(IPAddress.Parse("0.0.0.0"), port);
		listener.Start();

		Debug.Log("Awaiting connection on port " + port);
        soc = listener.AcceptSocket();

		Debug.Log("Received connection. Will begin to listen for messages");
		Listen();
    }

	// Set up stream reader and writer, then listen for messages.
	private void Listen()
	{
		try
		{
			NetworkStream stream = new NetworkStream(soc);
			StreamReader reader = new StreamReader(stream);
			StreamWriter writer = new StreamWriter(stream);
			writer.AutoFlush = true; // enable automatic flushing

			string message;
			while ((message = reader.ReadLine()) != null)
			{
				if (!string.IsNullOrEmpty(message))
				{
					Debug.Log(message);
					commandQueue.Enqueue(message);
				}
			}
		}
		catch (IOException e)
		{
			Debug.LogWarning("Error while listening for messages:");
			Debug.LogError(e);
		}

		commandQueue.Enqueue("close");
	}

	private void Close()
	{
		Debug.Log("Closing the socket and thread.");

		if(soc != null)
			soc.Close();

		listenThread.Abort();
		listenThread.Join();
	}

    public void OnDestroy()
    {
		Close();
    }

    private void Update()
    {
    	string cmd;
    	while(commandQueue.TryDequeue(out cmd))
    	{
    		//player.Jump();

    		switch(cmd)
    		{
    			case "Jump":
				case "jump":
    				player.Jump();
    				break;
    			case "Close":
				case "close":
    				Debug.Log("Closing safely, restarting resources.");
    				Close();
    				//PreSetup();
    				break;
				default:
					string[] floats = cmd.Split(',');
					float[] actualFloats = new float[floats.Length];

					for (int i = 0; i < floats.Length; ++i)
						actualFloats[i] = float.Parse(floats[i], CultureInfo.InvariantCulture);

                    Debug.Log(actualFloats[0] + ", " + actualFloats[1]);
                    pointer.Move(new Vector2(actualFloats[0], actualFloats[1]));
					break;
    		}
    	}
    }
}