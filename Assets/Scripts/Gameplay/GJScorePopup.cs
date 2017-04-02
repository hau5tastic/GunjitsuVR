using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GJScorePopup : MonoBehaviour {

    private GJLevel.GJAccuracy accuracy;
    private Color color;
    private Text text;

    private float animDuration;
    private float elapsed;
    Vector3 direction;
    Vector3 target;
    Vector3 velRef = Vector3.zero;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    
    void Start () {
        animDuration = GJLevel.instance.displayTime;
        Destroy(this.gameObject, animDuration);
        int i = Random.Range(0, 1);
        direction = (i == 0) ? Vector3.right : Vector3.left;
        target.Normalize();
        //Debug.Log("Position: " + transform.position);
        //Debug.Log("Target: " + target);
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

    void Animate()
    {

    }

    void Update()
    {
        if(elapsed < animDuration)
        {
            target = transform.position + new Vector3(1.0f, 1.0f, 0.0f);
            color.a -= 0.01f;
            text.color = color;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velRef, animDuration);
        }
    }
}
