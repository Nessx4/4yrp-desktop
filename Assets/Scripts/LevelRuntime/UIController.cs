using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public Text scoreText;
    public int score;
    public static UIController controller;
    public Text win;
    public GameObject health;

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

    public void Damage()
    {
        Destroy(health.transform.GetChild(0).gameObject);
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("sc_LevelRuntime");
    }
}
