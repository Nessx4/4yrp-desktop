using System;
using System.Collections;
using System.Collections.Generic;

using System.Runtime.InteropServices;

using UnityEngine;

public class TESTServer : MonoBehaviour 
{
	[DllImport("cs407_server")]
	private static unsafe extern void* server_start(string address);

	[DllImport("cs407_server")]
	private static unsafe extern void server_close(void* handle);

	private void Start()
	{
		unsafe
		{
			string address = "127.0.0.1:9000";

			void* handle = server_start(address);
			Debug.Log(new IntPtr(&handle).ToString());

			server_close(handle);
			Debug.Log("Server has closed.");
		}
	}
}
