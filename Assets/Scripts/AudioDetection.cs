using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioDetection : MonoBehaviour
{
    public int sampleWindow;
    private AudioClip micClip;

    // Start is called before the first frame update
    void Start()
    {
        
        MicToClip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MicToClip()
    {
        string micName = Microphone.devices[0];
        //Debug.Log("micName " + micName);
        micClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);

    }

    public float[] GetAbsAudioWaveFromMic()
    {
        //Debug.Log("micClip" + micClip.frequency);
        return GetAbsWaveFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
        //return 1.0f;
    }

    public float[] GetAudioWaveFromMic()
    {
        //Debug.Log("micClip" + micClip.frequency);
        return GetWaveFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
        //return 1.0f;
    }

    public float GetLoudnessFromMic()
    {
        //Debug.Log("micClip" + micClip.frequency);
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
        //return 1.0f;
    }

    public float[] GetFreqsFromMic()
    {
        //Debug.Log("micClip" + micClip.frequency);
        return GetFreqsFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), micClip);
        //return 1.0f;
    }

    public float GetLoudnessFromAudioClip(int index, AudioClip clip)
    {
        int clipSample = index - sampleWindow;
        if(clipSample < 0)
            clipSample = 0;

        float[] waveData = new float[sampleWindow];

        clip.GetData(waveData, clipSample);

        float totalLoudness = 0;

        for(int i = 0; i < waveData.Length; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        float[] absWaveData = new float[waveData.Length];
        for(int i = 0; i < waveData.Length; i++)
            absWaveData[i] = Mathf.Abs(waveData[i]);


        return totalLoudness;
    }

    public float[] GetWaveFromAudioClip(int index, AudioClip clip)
    {
        int clipSample = index - sampleWindow;
        if(clipSample < 0)
            clipSample = 0;

        float[] waveData = new float[sampleWindow];

        clip.GetData(waveData, clipSample);

        return waveData;
    }

    public float[] GetAbsWaveFromAudioClip(int index, AudioClip clip)
    {
        int clipSample = index - sampleWindow;
        if(clipSample < 0)
            clipSample = 0;

        float[] waveData = new float[sampleWindow];

        clip.GetData(waveData, clipSample);

        float[] absWaveData = new float[waveData.Length];
        for(int i = 0; i < waveData.Length; i++)
            absWaveData[i] = Mathf.Abs(waveData[i]);


        return absWaveData;
    }

    public float[] GetFreqsFromAudioClip(int index, AudioClip clip)
    {
        int clipSample = index - sampleWindow;
        if(clipSample < 0)
            clipSample = 0;

        float[] waveData = new float[sampleWindow];

        clip.GetData(waveData, clipSample);

        float totalLoudness = 0;

        for(int i = 0; i < waveData.Length; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }
        
        // insert Fast Fourier Transform
        float[] freqs = FFT.FFTtoFreq(FFT.GetFFT(waveData), waveData.Length);

        float pitch = FFT.ConvertToPitch(FFT.GetFFT(waveData));

        for(int i = 0; i < freqs.Length; i++)
        {
            freqs[i] = freqs[i] * 10;
        }

        return freqs;
    }
}
