using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{

    public bool cubeDid = false;
    public GameObject effectPrefab;
    public AudioClip cubDestoyAudio;
    
    private bool effectPlayed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cubeDid && !effectPlayed)
        {
            effectPlayed = true;
            StartCoroutine(PlaySoundAndEffectCoroutine());
            Destroy(gameObject);
        }
        transform.position += Time.deltaTime * transform.forward * 2;

        Destroy(gameObject,12);
    }


    private IEnumerator PlaySoundAndEffectCoroutine()
    {
        CubDestoySound();
        
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);
        Destroy(effect);
    }

    public void CubDestoySound(){
        GameObject soundObj = new GameObject("cubDestoySound");
        AudioSource audio = soundObj.AddComponent<AudioSource>();
        audio.clip = cubDestoyAudio;
        audio.Play();
        Destroy(soundObj, cubDestoyAudio.length);
    }
    

}
