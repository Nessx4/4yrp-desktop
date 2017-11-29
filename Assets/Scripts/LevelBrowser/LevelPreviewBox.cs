﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelPreviewBox : MonoBehaviour
{
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
	private Sprite fullStar;

	public void SetParameters(string name, Sprite image, int views, int rating)
	{
		levelName.text = name;
		levelSnapshot.sprite = image;
		viewCount.text = views.ToString();

		for(int i = 0; i < ratingStars.Length; ++i)
			ratingStars[i].sprite = (i < rating) ? fullStar : emptyStar;
	}
}
