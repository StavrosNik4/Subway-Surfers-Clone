using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;

    public GameObject panel;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI congratulationsText;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UpdateScore(0);

        panel.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        congratulationsText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Coffees: " + score + "/13";
    }

    public void GameOver(bool win)
    {
        panel.SetActive(true);

        if (win)
        {
            congratulationsText.gameObject.SetActive(true);
        }
        else
        {
            gameOverText.gameObject.SetActive(true);
        }
        
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public int getScore()
    {
        return score;
    }
}
