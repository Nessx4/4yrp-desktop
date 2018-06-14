using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelUpload : MonoBehaviour 
{
	private async void Start()
	{
		Debug.Log("Started the thing");

		await PollServer();

		Debug.Log("Ended the thing");
	}

	private async Task PollServer()
	{
		HttpClient client = new HttpClient();

		Task<string> getStringTask = client.GetStringAsync("http://danielilett.com");

		string urlContents = await getStringTask;

		Debug.Log(urlContents);

		return;
	}
}
