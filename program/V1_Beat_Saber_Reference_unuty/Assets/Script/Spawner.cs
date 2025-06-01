using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] cubes;
    public Transform[] points;
    public AudioSource audioForBlock;
    private int sampleWindow = 128;
    private float spawnThreshold = -20f; // 데시벨 임계값
    private float lastDecibel = -80f;
    private float triggerCooldown = 0.3f; // 최소 쿨타임
    private float triggerTimer = 0f;

    void Update()
    {
        triggerTimer += Time.deltaTime;
        if (audioForBlock.isPlaying)
        {
            float decibel = GetDecibel();
            Debug.Log("Decibel: " + decibel + " | Cooldown: " + triggerTimer);


            // 이전 데시벨보다 많이(특정값 이상) 상승했고, 쿨타임 지남
            if (decibel > spawnThreshold && lastDecibel <= spawnThreshold && triggerTimer > triggerCooldown)
            {
                Debug.Log("BLOCK SPAWNED!");
                GameObject cube = Instantiate(
                    cubes[Random.Range(0, cubes.Length)],
                    points[Random.Range(0, points.Length)]
                );
                cube.transform.localPosition = Vector3.zero;
                cube.transform.Rotate(transform.forward, 90 * Random.Range(0, 4));
                triggerTimer = 0f;
            }

            lastDecibel = decibel;

        }
    }

    float GetDecibel()
    {
        float[] waveData = new float[sampleWindow];
        int pos = audioForBlock.timeSamples;
        if (pos < sampleWindow) return -80f;

        audioForBlock.clip.GetData(waveData, pos - sampleWindow);

        float max = 0f;
        for (int i = 0; i < sampleWindow; ++i)
            max = Mathf.Max(max, Mathf.Abs(waveData[i]));

        return 20 * Mathf.Log10(max + 1e-7f);
    }
}
