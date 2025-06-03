using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public Text scoreText;

    public int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created





    void Update()
    {
        scoreText.text = "Point: " + score;
    }

    // Update is called once per frame


    public void ResetScore()
    {
        score = 0;
        SceneManager.LoadScene("GameScene");
    }

    public void AddScore(int a)
    {
        score += a;
    }
    
    
    

    
    
}