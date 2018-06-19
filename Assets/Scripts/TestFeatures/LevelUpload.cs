using System;

using System.IO;

using System.Net.Http;

using System.Threading;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelUpload : MonoBehaviour 
{
	private async void Start()
	{
		Debug.Log("Started the thing");

		FileStream stream = new FileStream(Application.persistentDataPath + "/testfile.txt", FileMode.Open);

		//await PollServer();
		await UploadAsync("testfile.txt", stream);

		Debug.Log("Ended the thing");
	}

	private async Task UploadAsync(string filename, Stream fileStream/*, byte[] fileBytes*/)
	{
		HttpContent stringContent = new StringContent(filename);
		HttpContent fileStreamContent = new StreamContent(fileStream);
		//HttpContent bytesContent = new ByteArrayContent(fileBytes);

		using (var client = new HttpClient())
		using(var formData = new MultipartFormDataContent())
		{
			// Create a HTTP form containing the file upload.
			formData.Add(stringContent, "filename", "filename");
			formData.Add(fileStreamContent, "file1", "file1");

			int timeout = 30 * 1000;

			CancellationTokenSource source = new CancellationTokenSource();
			source.CancelAfter(TimeSpan.FromSeconds(30));

			var response = Task.Run(() => client.PostAsync("http://danielilett.com/request.php", formData), source.Token);

			if(await Task.WhenAny(response, Task.Delay(timeout, source.Token)) == response)
			{
				try
				{
					HttpResponseMessage message = await response;
					string content = await message.Content.ReadAsStringAsync();

					Debug.Log(content);
				}
				catch(Exception e)
				{
					Debug.LogError(e);
					return;
				}
			}
			else
			{
				Debug.Log("Timed out");
			}
		}

		return;
	}

	private async Task PollServer()
	{
		HttpClient client = new HttpClient();

		int timeout = 30 * 1000;

		CancellationTokenSource source = new CancellationTokenSource();
		source.CancelAfter(TimeSpan.FromSeconds(30));

		Task<string> getStringTask = Task.Run(() => client.GetStringAsync("http://danielilett.com/request.php"), source.Token);

		if(await Task.WhenAny(getStringTask, Task.Delay(timeout, source.Token)) == getStringTask)
		{
			try
			{
				string urlContents = await getStringTask;
				Debug.Log(urlContents);
			}
			catch(Exception e)
			{
				Debug.LogError(e);
				return;
			}
		}
		else
		{
			Debug.Log("Timed out");
		}

		return;
	}
}
