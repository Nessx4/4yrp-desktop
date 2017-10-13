using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

public class Prototype : MonoBehaviour
{
    public Text prototext;
    private Thread t;
    static TcpListener listener;
    private Socket soc;

    public void Start()
    {
        Run();
    }

    public void Run()
    {
        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
        for (int i = 0; i < localIPs.Length; i++)
        {
            Debug.Log(localIPs[i]);
        }
        listener = new TcpListener(IPAddress.Parse("0.0.0.0"), 80);
        listener.Start();
        t = new Thread(new ThreadStart(Service));
        t.Start();
    }
    public void Service()
    {
    Debug.Log("Hello I'm listening for a connection");
        soc = listener.AcceptSocket();
        Debug.Log("Got a connection");
        //soc.SetSocketOption(SocketOptionLevel.Socket,
        //        SocketOptionName.ReceiveTimeout,10000);
        try
        {
            Stream s = new NetworkStream(soc);
            StreamReader sr = new StreamReader(s);
            StreamWriter sw = new StreamWriter(s);
            sw.AutoFlush = true; // enable automatic flushing

            while(true)
            {
                string message = sr.ReadLine();
                if (!string.IsNullOrEmpty(message))
                    Debug.Log(message);
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("AHHHHHHHHHHHHH");
        }
        soc.Close();
    }

    public void OnDestroy()
    {

        soc.Close();

        t.Join();
    }
}