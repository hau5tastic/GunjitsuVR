using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GJScore;


public class GJScorePopup : MonoBehaviour {

    private GJAccuracy accuracy;
    private Color color;
    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start () {      
        Destroy(this.gameObject, GameSettings.displayTime);
	}

    public void Init(GJAccuracy _accuracy)
    {
        accuracy = _accuracy;
        switch(accuracy)
        {
            case GJAccuracy.PERFECT:
                color = GameSettings.perfectColor;
                text.text = "Perfect!";
                break;
            case GJAccuracy.GREAT:
                color = GameSettings.greatColor;
                text.text = "Great!";
                break;
            case GJAccuracy.GOOD:
                color = GameSettings.goodColor;
                text.text = "Good";
                break;
            case GJAccuracy.OK:
                color = GameSettings.okColor;
                text.text = "OK";
                break;
            default:
                color = Color.white;
                break;
        }

        text.color = color;
    }
}
