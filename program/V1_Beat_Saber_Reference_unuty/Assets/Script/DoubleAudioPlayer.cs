using UnityEngine;

public class DoubleAudioPlayer : MonoBehaviour
{
    public AudioSource audioForPlay;  // 플레이어가 듣는 소스
    public AudioSource audioForBlock; // 분석(음소거)
    private float blockDelay = 5.2f;     // 분석용 곡 딜레이(초)
    private bool blockEnded = false;
    public int beatIndex = 0;

    void Start()
    {
        audioForBlock.Play();
        Invoke(nameof(PlayBlockAudio), blockDelay);
    }

    void PlayBlockAudio()
    {
        audioForPlay.Play();
    }
    void Update()
    {
        // 곡이 끝났고, 아직 처리 안 했으면
        if (!blockEnded && audioForBlock.time > 0 && !audioForBlock.isPlaying)
        {
            beatIndex++;
            blockEnded = true;
            Debug.Log("노래가 끝났음! Beat Index 증가: " + beatIndex);

            // 추가로, 콜백이나 이벤트 처리도 여기에!
            // OnSongEnd?.Invoke();
        }
    }
}
