using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SpotlightIntro : MonoBehaviour {

    public float startAngle;
    public float endAngle;
    public float deltaPerSec;

    new Light light;

	void Start () {
        light = GetComponent<Light>();
        light.spotAngle = startAngle;
	}

	void Update () {
        if (light.spotAngle < endAngle) {
            light.spotAngle = light.spotAngle + deltaPerSec * Time.deltaTime;
        }
	}
}
