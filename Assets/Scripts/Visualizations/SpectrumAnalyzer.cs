using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*
* Code created with the help of @PeerPlay tutorials
* https://www.youtube.com/playlist?list=PL3POsQzaCw53p2tA6AWf7_AWgplskR0Vo
*/

[RequireComponent(typeof(AudioSource))]
public class SpectrumAnalyzer : MonoBehaviour
{
    AudioSource audioSource;

    float[] spectrum = new float[1024];
    float[] spectrumBand = new float[8];
    public float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    float[] highestFreqBand = new float[8];
    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        MakeFrequencyBands();
        BandBuffer();
        NormalizeAudioBands();
    }

    void MakeFrequencyBands()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);

        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0.0f;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            for (int j = 0; j < sampleCount; j++)
            {
                average += spectrum[count] * (count + 1);
                count++;
            }

            average /= count;
            spectrumBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (spectrumBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = spectrumBand[i];
                bufferDecrease[i] = 0.005f;
            }

            if (spectrumBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void NormalizeAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (spectrumBand[i] > highestFreqBand[i])
            {
                highestFreqBand[i] = spectrumBand[i];
            }
            audioBand[i] = spectrumBand[i] / highestFreqBand[i];
            audioBandBuffer[i] = bandBuffer[i] / highestFreqBand[i];
        }
    }

    public float GetBandBuffer(int index)
    {
        return bandBuffer[index];
    }
}