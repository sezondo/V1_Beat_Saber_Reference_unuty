using UnityEngine;

public class MicDecibelReader : MonoBehaviour
{
    public float decibel; // 외부에서 읽기

    AudioClip micClip;
    int sampleWindow = 128;
    string micName;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 1, 44100);
        }
    }

    void Update()
    {
        decibel = GetMaxVolume();
    }

    float GetMaxVolume()
    {
        float max = 0f;
        float[] waveData = new float[sampleWindow];
        int micPos = Microphone.GetPosition(micName) - sampleWindow + 1;
        if (micPos < 0) return 0;

        micClip.GetData(waveData, micPos);
        for (int i = 0; i < sampleWindow; ++i)
        {
            float wavePeak = Mathf.Abs(waveData[i]);
            if (max < wavePeak)
            {
                max = wavePeak;
            }
        }
        // dB 변환(간단화 버전, 0~1 구간)
        return 20 * Mathf.Log10(max + 1e-7f);
    }
}
