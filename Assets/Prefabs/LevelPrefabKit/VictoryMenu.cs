using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

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
        }
        textAccuracy.text = "Accuracy: " + accuracy + "%";

        if (fortune < newFortune) {
            float sfxOffset = 2.0f; // because the sfx doesnt exactly end to its length
            float accumulation = (float)newFortune / (SFXAccumulate.length - sfxOffset);
            fortune += (int)(accumulation * Time.deltaTime);
        } else if (fortune >= newFortune) {
            fortune = newFortune;
        }
        textFortune.text = "Fortune: $ " + fortune;
    }

    private void OnEnable()
    {
        //StackTrace st = new StackTrace(new StackFrame(true));
        //UnityEngine.Debug.Log("WTF IS GOING ON? Stack trace for current level: ");
        ////StackFrame sf = st.GetFrame(0);
        //Debug.Log(" File: {0}", sf.GetFileName());
        //Debug.Log(" Method: {0}", sf.GetMethod().Name);
        //Debug.Log(" Line Number: {0}", sf.GetFileLineNumber());
        //Debug.Log(" Column Number: {0}", sf.GetFileColumnNumber());
    }
}
