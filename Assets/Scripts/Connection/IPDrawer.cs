using System.Net;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class IPDrawer : MonoBehaviour 
{
	[SerializeField]
	private Text ipTextPrefab;

	[SerializeField]
	private Transform drawerRoot;

	public void AddAddress(IPAddress address)
	{
		Text newObject = Instantiate<Text>(ipTextPrefab, drawerRoot);
		newObject.text = address.ToString();
	}

	public void AddAddress(string address)
	{
		Text newObject = Instantiate<Text>(ipTextPrefab, drawerRoot);
		newObject.text = address;
	}

	public void RemoveAddresses()
	{
		for(int i = 2; i < drawerRoot.childCount; ++i)
			Destroy(drawerRoot.GetChild(i).gameObject);
	}
}
