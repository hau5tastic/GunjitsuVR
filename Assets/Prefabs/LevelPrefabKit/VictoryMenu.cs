using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class VictoryMenu : MonoBehaviour {

    public AudioClip SFXAccumulate;

    [SerializeField]
    Text title;
    [SerializeField]
    Text textAccuracy;
    [SerializeField]
    Text textFortune;

    float accuracy = 0;
    int fortune = 0;

    void Start() {
        GetComponent<AudioSource>().PlayOneShot(SFXAccumulate);
    }

    void Update() {
        if (accuracy < GJLevel.instance.accuracy) {
            accuracy+=0.2f;
        } else
        {
            accuracy = GJLevel.instance.accuracy;
        }
        float displayAccuracy = Mathf.Round(accuracy * 100f) / 100f;
        textAccuracy.text = "Accuracy: " + displayAccuracy + "%";

        if (fortune < GJLevel.instance.fortune) {
            float sfxOffset = 2.0f; // because the sfx doesnt exactly end to its length
            float accumulation = (float)GJLevel.instance.fortune / (SFXAccumulate.length - sfxOffset);
            fortune += (int)(accumulation * Time.deltaTime);
        } else if (fortune >= GJLevel.instance.fortune) {
            fortune = GJLevel.instance.fortune;
        }
        textFortune.text = "Fortune: $ " + fortune;
    }

    public void SetTitle(string text, Color color) {
        title.text = text;
        title.color = color;
    }

    private void OnEnable()
    {
        GetComponent<Canvas>().worldCamera = ViveControllerInput.Instance.ControllerCamera;
    }
}
