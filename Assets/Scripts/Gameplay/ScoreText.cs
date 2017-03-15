using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    public static ScoreText reference;

    Text scoreText;
    int score = 0;

	void Start () {
        reference = this;
        scoreText = GetComponent<Text>();
	}

    void Update()
    {
        //scoreText.text = "Score: " + score;
    }

	public void AddScore(int val)
    {
        score += val;
    }
}
