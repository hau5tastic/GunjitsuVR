using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GJScore;


public class GJScorePopup : MonoBehaviour {

    public ScoreSettings settings = new ScoreSettings();

    private GJAccuracy accuracy;
    private Color color;

	void Start () {
        Destroy(this.gameObject, settings.displayTime);
	}

    public void Init(GJAccuracy _accuracy)
    {
        accuracy = _accuracy;
        switch(accuracy)
        {
            case GJAccuracy.PERFECT:
                color = settings.perfectColor;
                break;
            case GJAccuracy.GREAT:
                color = settings.greatColor;
                break;
            case GJAccuracy.GOOD:
                color = settings.goodColor;
                break;
            case GJAccuracy.OK:
                color = settings.okColor;
                break;
            default:
                color = Color.white;
                break;
        }
    }
}
