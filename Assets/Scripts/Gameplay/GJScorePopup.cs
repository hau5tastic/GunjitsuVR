using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GJScorePopup : MonoBehaviour {

    private GJLevel.GJAccuracy accuracy;
    private Color color;
    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    
    void Start () {      
        Destroy(this.gameObject, GJLevel.instance.displayTime);
	}

    public void Init(GJLevel.GJAccuracy _accuracy)
    {
        accuracy = _accuracy;
        switch(accuracy)
        {
            case GJLevel.GJAccuracy.PERFECT:
                color = GJLevel.instance.perfectColor;
                text.text = "Perfect!";
                ScoreText.reference.AddScore(GJLevel.instance.perfectScore);
                break;
            case GJLevel.GJAccuracy.GREAT:
                color = GJLevel.instance.greatColor;
                text.text = "Great!";
                ScoreText.reference.AddScore(GJLevel.instance.greatScore);
                break;
            case GJLevel.GJAccuracy.GOOD:
                color = GJLevel.instance.goodColor;
                text.text = "Good";
                ScoreText.reference.AddScore(GJLevel.instance.goodScore);
                break;
            case GJLevel.GJAccuracy.OK:
                color = GJLevel.instance.okColor;
                text.text = "OK";
                ScoreText.reference.AddScore(GJLevel.instance.okScore);
                break;
            default:
                color = Color.white;
                break;
        }

        text.color = color;
    }
}
