using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    public static ScoreText reference;

    Text scoreText;
    int score = 0;

    void Awake()
    {
        reference = this;
    }

	void Start () {
        scoreText = GetComponent<Text>();
	}

    void Update()
    {
        scoreText.text = score.ToString();
        GJLevel.instance.fortune = score;
    }

	public void AddScore(int val)
    {
        score += val;
    }

    public void Reset()
    {
        score = 0;
    }
}
