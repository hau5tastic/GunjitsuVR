using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerObject : MonoBehaviour
{
    public int band;
    public float startScale, maxScale;
    public bool useBuffer;
    Material material;

    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (SpectrumAnalyzer.audioBandBuffer[band] * maxScale) + startScale, transform.localScale.z);
            Color color = new Color(SpectrumAnalyzer.audioBandBuffer[band], SpectrumAnalyzer.audioBandBuffer[band], SpectrumAnalyzer.audioBandBuffer[band]);
            material.SetColor("_EmissionColor", color);
        }
        if (!useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (SpectrumAnalyzer.audioBand[band] * maxScale) + startScale, transform.localScale.z);
            Color color = new Color(SpectrumAnalyzer.audioBand[band], SpectrumAnalyzer.audioBand[band], SpectrumAnalyzer.audioBand[band]);
            material.SetColor("_EmissionColor", color);
        }
    }
}