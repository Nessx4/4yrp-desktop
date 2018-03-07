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
    public UFO ufo;

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
                    mobileID = PointerController.instance.CreateMobilePointer();
                    break;
    			case "Close":
				case "close":
    				Debug.Log("Closing safely, restarting resources.");
    				Close();
    				//PreSetup();
    				break;
                case "action_start":
                    //TileDraw.placement.StartMobileDraw();
                    break;
                case "action_end":
                    //TileDraw.placement.StopMobileDraw();
                    break;
                case "capture":
                    PointerController.instance.MakeInvisible(mobileID);
                    break;
                case "leave":
                    PointerController.instance.MakeVisible(mobileID);
                    break;
                case "left":
                    ufo.Move(Vector2.left);
                    break;
                case "right":
                    ufo.Move(Vector2.right);
                    break;
                case "stop":
                    ufo.Move(new Vector2(0, 0));
                    break;
                case "undo":
					CreatorPlayerWrapper.Get().Undo(1);
                    Debug.Log("undo");
                    break;
                case "redo":
					CreatorPlayerWrapper.Get().Redo(1);
                    Debug.Log("redo");
                    break;
                case "pencil":
                    CreatorPlayerWrapper.Get().SetActiveTool(1, ToolType.PENCIL);
                    throw new System.NotImplementedException("Start mobile draw.");
                    //TileDraw.placement.StartMobileDraw();
                    //break;
                case "pencil_end":
                    throw new System.NotImplementedException("Stop mobile draw.");
                    //TileDraw.placement.StopMobileDraw();
                    //break;
                case "eraser":
                    CreatorPlayerWrapper.Get().SetActiveTool(1, ToolType.ERASER);
                    throw new System.NotImplementedException("Start mobile erase.");
                    //TileDraw.placement.StartMobileDraw();
                    //break;
                case "eraser_end":
                    throw new System.NotImplementedException("Stop mobile erase.");
                    //TileDraw.placement.StopMobileDraw();
                    //break;
                case "basic 0":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[0]);
                    break;
                case "basic 1":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[1]);
                    break;
                case "basic 2":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[2]);
                    break;
                case "basic 3":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[3]);
                    break;
                case "bg 0":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[4]);
                    break;
                case "bg 1":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[5]);
                    break;
                case "bg 2":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[6]);
                    break;
                case "bg 3":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[7]);
                    break;
                case "bg 4":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[8]);
                    break;
                case "tech 0":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[9]);
                    break;
                case "misc 0":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[10]);
                    break;
                case "misc 1":
                    CreatorPlayerWrapper.Get().SetActiveTile(1, tiles[11]);
                    break;
                default:
					string[] floats = cmd.Split(',');
					float[] actualFloats = new float[floats.Length];

					for (int i = 0; i < floats.Length; ++i)
						actualFloats[i] = float.Parse(floats[i], CultureInfo.InvariantCulture);

                    Debug.Log(actualFloats[0] + ", " + actualFloats[1]);
                    PointerController.instance.MovePointer(mobileID, new Vector2(actualFloats[0], actualFloats[1]));
					break;
    		}
    	}
    }

    // Strange esoteric C# event stuff.

    /*
    private MobileEvent _lastEvent;

    public MobileEvent LastEvent
    {
        get {return _lastEvent; }
        set
        {
            if(_lastEvent != value)
            {
                _lastEvent = value;
                Notify();
            }
        }
    }

    private void Notify()
    {
        if(_onChange != null)
            _onChange(_lastEvent);
    }

    private event MobileEventHandler _onChange;

    public event MobileEventHandler OnMobileEvent
    {
        add
        {
            _onChange += value;
        }
        remove
        {
            _onChange -= value;
        }
    }
    */
}

//public delegate void MobileEventHandler(MobileEvent mEvent);

public class MobileEvent
{

}

public class CursorMoveEvent : MobileEvent
{
    Vector2 move;

    public CursorMoveEvent(Vector2 move)
    {
        this.move = move;
    }
}

public class ButtonEvent : MobileEvent
{
    
}