using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using UnityEngine;
using UnityEngine.UI;

using System.Threading;

using System.IO;
using System.Net;
using System.Net.Sockets;

public class Prototype : MonoBehaviour
{
	[SerializeField]
	private RobotControllerScript player;

	// Network communication.
    private Thread listenThread;
    private TcpListener listener;
    private Socket soc;

	private int port = 9000;

	private ConcurrentQueue<Command> qu;

	// Begin a new thread to start listening on a socket.
    private void Start()
    {
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

					// Need to work out how to send messages back to the main thread.
					switch (message)
					{
						case "jump":
							qu.Enqueue(new JumpCommand());
							break;
						default:
							Debug.Log(message);
							break;
					}
				}
			}
		}
		catch (IOException e)
		{
			Debug.LogWarning("Error while listening for messages:");
			Debug.LogError(e);
		}

		Close();
	}

	private void Close()
	{
		Debug.Log("Closing stuff");

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
    	while((Command c = qu.Dequeue()) != null)
    	{
    		player.Jump();
    	}
    }
}

private class Command
{

}

private class JumpCommand : Command
{

}