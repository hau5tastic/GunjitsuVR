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
        Destroy(this.gameObject, GJLevel.instance.displayTime);
	}

    public void Init(GJAccuracy _accuracy)
    {
        accuracy = _accuracy;
        switch(accuracy)
        {
            case GJAccuracy.PERFECT:
                color = GJLevel.instance.perfectColor;
                text.text = "Perfect!";
                break;
            case GJAccuracy.GREAT:
                color = GJLevel.instance.greatColor;
                text.text = "Great!";
                break;
            case GJAccuracy.GOOD:
                color = GJLevel.instance.goodColor;
                text.text = "Good";
                break;
            case GJAccuracy.OK:
                color = GJLevel.instance.okColor;
                text.text = "OK";
                break;
            default:
                color = Color.white;
                break;
        }

        text.color = color;
    }
}
