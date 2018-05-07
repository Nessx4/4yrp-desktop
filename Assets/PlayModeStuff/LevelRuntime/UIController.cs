using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text scoreText;
    public int score;
    public static UIController controller;
    public Text win;

	void Start () {
        controller = this;
        score = 0;
        scoreText.text = score.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Score(int score)
    {
        this.score += score;
        scoreText.text = score.ToString();
    }

    public void Goal()
    {
        win.text = "You won!";
    }
}
