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
        UpdateScoreAndText();

    }

    void UpdateScoreAndText()
    {
        scoreText.text = score.ToString();
        GJLevel.instance.fortune = score;
    }

	public void AddScore(int val)
    {
        score += val;
        UpdateScoreAndText();
    }

    public void Reset()
    {
        score = 0;
        UpdateScoreAndText(); 
    }
}
