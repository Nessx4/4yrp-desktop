using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationBox : MonoBehaviour 
{
	[SerializeField] private Notification notificationPrefab;

	[SerializeField] private Transform notificationArea;

	private string[] notifications = 
	{
		"You reached 10,000 total views!",
		"<color=#00afff>john_smith2834</color> likes your level, <color=#ff1f00>Super Patrick World</color>.",
		"Version 0.1.0 has launched."
	};

	[SerializeField] private Sprite[] sprites;

	private void Start()
	{
		for(int i = 0; i < notifications.Length; ++i)
		{
			Notification newNotification = Instantiate(notificationPrefab);
			newNotification.transform.SetParent(notificationArea);

			newNotification.SetText(notifications[i]);
			newNotification.SetIcon(sprites[i]);
		}
	}
}
