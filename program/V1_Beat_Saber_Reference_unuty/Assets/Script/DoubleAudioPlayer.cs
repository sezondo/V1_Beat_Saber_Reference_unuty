using UnityEngine;

public class DoubleAudioPlayer : MonoBehaviour
{
    public AudioSource audioForPlay;  // 플레이어가 듣는 소스
    private float blockDelay = 7f;     // 분석용 곡 딜레이(초)
    private bool blockEnded = false;

    void Start()
    {
        
        Invoke(nameof(PlayBlockAudio), blockDelay);
    }

    void PlayBlockAudio()
    {
        audioForPlay.Play();
    }
    void Update()
    {
        // 곡이 끝났고, 아직 처리 안 했으면
        if (!blockEnded && !audioForPlay.isPlaying)
        {
            blockEnded = true;
            
        }
    }
}
