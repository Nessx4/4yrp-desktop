using System.Net;
using System.Net.Sockets;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
	// A separate camera is used to draw each pointer.
	[SerializeField]
	private Camera pointerCamera;

	[SerializeField]
	private IPDrawer drawer;

	private const int startPort = 9000;
	
	private int createdConnections = 0;
	private TcpListener listener;

	[SerializeField]
	private MobileConnection connection;

	public static ConnectionManager instance { get; private set; }

	private void Awake()
	{
		Manager.manager = this;
		instance = this;

		listener = new TcpListener(IPAddress.Any, startPort);
		listener.Start();

		IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

		for (int i = 0; i < localIPs.Length; ++i)
			drawer.AddAddress(localIPs[i]);

		Manager.LoadScene("sc_TitleScreen");
		CreateNewConnection();
	}

	public void CreateNewConnection()
	{
		if(createdConnections < 4)
		{
			var conn = Instantiate<MobileConnection>(connection, Vector3.zero, 
				Quaternion.identity, transform);

			conn.SetupThreads(createdConnections++, listener, true);
			conn.mobileCamera = pointerCamera;
		}
	}
}
