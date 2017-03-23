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
                break;
            case GJLevel.GJAccuracy.GREAT:
                color = GJLevel.instance.greatColor;
                text.text = "Great!";
                break;
            case GJLevel.GJAccuracy.GOOD:
                color = GJLevel.instance.goodColor;
                text.text = "Good";
                break;
            case GJLevel.GJAccuracy.OK:
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
