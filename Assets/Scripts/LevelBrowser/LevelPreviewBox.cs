using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BrowserButton))]
public class LevelPreviewBox : MonoBehaviour
{
	private long timestamp;

	[SerializeField]
	private Text levelName;

	[SerializeField]
	private Image levelSnapshot;

	[SerializeField]
	private Text viewCount;

	[SerializeField]
	private Image[] ratingStars;

	[SerializeField]
	private Sprite emptyStar;

	[SerializeField]
	private Sprite halfStar;

	[SerializeField]
	private Sprite fullStar;

	public void SetParameters(string name, string filename, Sprite image, int views, int rating, long timestamp)
	{
		levelName.text = name;
		levelSnapshot.sprite = image;
		viewCount.text = views.ToString();

		for(int i = 0; i < ratingStars.Length; ++i)
		{
			ratingStars[i].sprite = (i < rating / 2) ? fullStar : 
				((i < rating / 2 + 1) ? halfStar : emptyStar);
		}

		this.timestamp = timestamp;

		GetComponent<BrowserButton>().filename = filename;
	}

	public long GetTimestamp()
	{
		return timestamp;
	}
}
