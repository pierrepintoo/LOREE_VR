using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedController : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] Material ledMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float[] spectrum = new float[256];
        audio.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        float sum = 0.0f;
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            sum += spectrum[i];
        }
        float moyenne = sum / spectrum.Length;
        float heightLeds = moyenne * 100;
        // float volume = audio.getVolume();
        // ledMaterial.SetFloat("_Fill", Mathf.Lerp(0.0f, 1.0f, heightLeds));
        ledMaterial.SetFloat("_Fill", heightLeds);
        Debug.Log(heightLeds);
    }
}
