using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class VisualizerLight : MonoBehaviour
{
    public int band;
    public float minIntensity, maxIntensity;
    new Light light;

    void Start()
    {
        light = GetComponent<Light>();
    }

    void Update()
    {
        light.intensity = (SpectrumAnalyzer.audioBandBuffer[band] * (maxIntensity - minIntensity)) + minIntensity;
    }
}
