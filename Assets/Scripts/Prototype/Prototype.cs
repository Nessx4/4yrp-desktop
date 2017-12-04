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
    private Pointer pointer;

    private int mobileID;

    // Network communication.
    private Thread listenThread;
    private TcpListener listener;
    private Socket soc;

	private int port = 9000;

	private ConcurrentQueue<string> commandQueue;

    private static Prototype proto;

    [SerializeField]
    private List<TileData> tiles;

	// Begin a new thread to start listening on a socket.
    private void Start()
    {
        if (proto == null)
        {
            proto = this;
            DontDestroyOnLoad(gameObject);
            PreSetup();
        }
        else Destroy(gameObject);
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

        commandQueue.Enqueue("new_pointer");

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

    		switch(cmd)
    		{
                case "new_pointer":
                    mobileID = PointerController.control.CreateMobilePointer();
                    break;
    			case "Close":
				case "close":
    				Debug.Log("Closing safely, restarting resources.");
    				Close();
    				//PreSetup();
    				break;
                case "undo":
                    Debug.Log("undo");
                    break;
                case "redo":
                    Debug.Log("redo");
                    break;
                case "pencil":
                    TilePlacement.placement.SetActiveToolMobile(ToolType.PENCIL);
                    TilePlacement.placement.StartMobileDraw();
                    break;
                case "pencil_end":
                    TilePlacement.placement.StopMobileDraw();
                    break;
                case "eraser":
                    TilePlacement.placement.SetActiveToolMobile(ToolType.ERASER);
                    TilePlacement.placement.StartMobileDraw();
                    break;
                case "eraser_end":
                    TilePlacement.placement.StopMobileDraw();
                    break;
                case "tile 0":
                    TilePlacement.placement.SetActiveTileMobile(tiles[0]);
                    break;
                case "tile 1":
                    TilePlacement.placement.SetActiveTileMobile(tiles[1]);
                    break;
                case "tile 2":
                    TilePlacement.placement.SetActiveTileMobile(tiles[2]);
                    break;
                case "tile 3":
                    TilePlacement.placement.SetActiveTileMobile(tiles[3]);
                    break;
                case "tile 4":
                    TilePlacement.placement.SetActiveTileMobile(tiles[4]);
                    break;
                case "tile 5":
                    TilePlacement.placement.SetActiveTileMobile(tiles[5]);
                    break;
                case "tile 6":
                    TilePlacement.placement.SetActiveTileMobile(tiles[6]);
                    break;
                case "tile 7":
                    TilePlacement.placement.SetActiveTileMobile(tiles[7]);
                    break;
                case "tile 8":
                    TilePlacement.placement.SetActiveTileMobile(tiles[8]);
                    break;
                case "tile 9":
                    TilePlacement.placement.SetActiveTileMobile(tiles[9]);
                    break;
                default:
					string[] floats = cmd.Split(',');
					float[] actualFloats = new float[floats.Length];

					for (int i = 0; i < floats.Length; ++i)
						actualFloats[i] = float.Parse(floats[i], CultureInfo.InvariantCulture);

                    Debug.Log(actualFloats[0] + ", " + actualFloats[1]);
                    PointerController.control.MovePointer(mobileID, new Vector2(actualFloats[0], actualFloats[1]));
					break;
    		}
    	}
    }
}