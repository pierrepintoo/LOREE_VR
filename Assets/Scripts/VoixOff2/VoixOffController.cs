using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEditor.VFX;
// using UnityEditor.Experimental.Rendering.HDPipeline;
using UnityEditor.VFX.UI;

public class VoixOffController : MonoBehaviour
{
    public VisualEffect _visualEffect;
    AudioSource _audioSource;
    
    public static float[] _samples = new float[512];

    float[] _freqBand = new float[8];

    float[] _bandBuffer = new float[8];

    float[] _bufferDecrease = new float[8];

    float[] _freqBandHighest = new float[8];

    public static float[] _audioBand = new float[8];

    public static float[] _audioBandBuffer = new float[8];

    public static float _Amplitude, _AmplitudeBuffer;

    float AmplitudeHighest;
    // Start is called before the first frame update
    void Start()
    {
        _visualEffect = GetComponent<UnityEngine.VFX.VisualEffect>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Upda te is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBand();
        GetAmplitude();
        _visualEffect.SetFloat("Volume", Mathf.Exp(_Amplitude * 4));
        // _visualEffect.SetFloat("Shiny", Mathf.Max(_Amplitude, 0.2f));
    }

    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++) {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }

        if (_CurrentAmplitude > AmplitudeHighest) {
            AmplitudeHighest = _CurrentAmplitude;
        }

        _Amplitude = _CurrentAmplitude / AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / AmplitudeHighest;
    }

    void CreateAudioBand()
    {

        for (int i = 0; i < 8; i++) {
            if (_freqBand[i] > _freqBandHighest[i]) {
                _freqBandHighest[i] = _freqBand[i];
            }

            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_freqBand[i] / _freqBandHighest[i]);
        }

    }

    void BandBuffer()
    {
        for (int g = 0; g < 8; g++) {
            if (_freqBand[g] > _bandBuffer[g]) {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }

            if (_freqBand[g] < _bandBuffer[g]) {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++) {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7) {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++) {
                average += _samples[count] * (count + 1);
                count++;
            }

            average = average / count;

            _freqBand[i] = average * 10;
        }

  
    }
}
