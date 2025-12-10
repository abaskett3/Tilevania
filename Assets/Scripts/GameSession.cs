using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI livesText;

    [SerializeField]
    TextMeshProUGUI scoreText;

    private int playerLives = 3;
    private int score = 0;
    private int reloadedGameScene = 0;

    //happens before everything else
    void Awake()
    {
        Debug.Log("Initializing New GameSession");
        //Create our singleton
        int numberOfGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numberOfGameSessions > 1)
        {
            Debug.Log("Multiple Game Sessions found. Removing newly creating GameSession.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("PlayerLives: " + playerLives);
            Debug.Log("score: " + score);
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = this.playerLives.ToString();
        scoreText.text = this.score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            Debug.Log("Processing Player Death.");
            TakeLife();
        }
        else
        {
            Debug.Log("No Lives left, restarting game.");
            ResetGameSession();
        }
    }

    public void IncrementScore(int score)
    {
        Debug.Log("Incrementing score by: " + score.ToString());
        this.score += score;
        scoreText.text = this.score.ToString();
    }

    private void ResetGameSession()
    {
        Debug.Log("Removing GameSession singleton due to game restart.");
        FindAnyObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(reloadedGameScene);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        this.playerLives--;
        livesText.text = this.playerLives.ToString();
        Debug.Log("Decrementing Lives. Lives Left: " + playerLives);
        Debug.Log("Restarting Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
