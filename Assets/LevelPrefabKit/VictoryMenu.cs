using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour {

    public AudioClip SFXAccumulate;

    [SerializeField]
    Text textAccuracy;

    [SerializeField]
    Text textFortune;

    public float newAccuracy = 0;
    public int newFortune = 0;
    float accuracy = 0;
    int fortune = 0;

    void Start() {
        GetComponent<AudioSource>().PlayOneShot(SFXAccumulate);
    }

    void Update() {
        if (accuracy < newAccuracy) {
            accuracy+=0.2f;
            accuracy = Mathf.Round(accuracy * 100f) / 100f;
            textAccuracy.text = "Accuracy: " + accuracy + "%";
        }

        if (fortune < newFortune) {
            float sfxOffset = 2.0f; // because the sfx doesnt exactly end to its length
            float accumulation = (float)newFortune / (SFXAccumulate.length - sfxOffset);
            fortune += (int)(accumulation * Time.deltaTime);
            textFortune.text = "Fortune: $ " + fortune;
        } else if (fortune >= newFortune) {
            fortune = newFortune;
            textFortune.text = "Fortune: $ " + fortune;
        }
    }
}
