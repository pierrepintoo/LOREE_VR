using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] Material ledMaterial;
    [SerializeField] float fillMultiplier;
    float[] samples = new float[512];
    float[] freqBand = new float[8];
    public float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ======== OLD CODE ========
        // float[] spectrum = new float[256];
        // audio.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        // float sum = 0.0f;
        // for (int i = 1; i < spectrum.Length - 1; i++)
        // {
        //     sum += spectrum[i];
        // }
        // float moyenne = sum / spectrum.Length;
        // float heightLeds = moyenne * 100;
        // ledMaterial.SetFloat("_Fill", heightLeds);
        // Debug.Log(heightLeds);
        // ==========================

        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        ledMaterial.SetFloat("_Fill", bandBuffer[0] * fillMultiplier);
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (freqBand[g] > bandBuffer[g])
            {
                bandBuffer[g] = freqBand[g];
                bufferDecrease[g] = 0.005f;
            }

            if (freqBand[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7) {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }
    }
}
