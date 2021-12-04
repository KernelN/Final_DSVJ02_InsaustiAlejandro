using UnityEngine;
using TMPro;
using System.Collections;

public class UILevelManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject HUD;
    [SerializeField] TextMeshProUGUI spawnTimerText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;
    //this is ugly, clean later v
    [SerializeField] TextMeshProUGUI defeatTimerText;
    [SerializeField] TextMeshProUGUI defeatScoreText;
    [SerializeField] TextMeshProUGUI victoryTimerText;
    [SerializeField] TextMeshProUGUI victoryScoreText;
    //this is ugly, clean later ^
    GameManager gameManager;

    //Unity Events
    private void Start()
    {
        gameManager = GameManager.Get();
        
        //Link Actions
        levelManager.PlayerDied += OnPlayerDied;
        levelManager.PlayerWon += OnPlayerWon;
        levelManager.PlayerLost += OnPlayerLost;
        levelManager.ScoreUpdated += OnScoreUpdated;

        //Set first values of evertything
        FirstSet();
    }
    private void LateUpdate()
    {
        SetTimerText(timerText);
    }

    //Methods
    public void Pause()
    {
        HUD.SetActive(false);
        pausePanel.SetActive(true);
        gameManager.SetPause(0);
    }
    public void UnPause()
    {
        HUD.SetActive(true);
        pausePanel.SetActive(false);
        gameManager.SetPause(1);
    }
    void SetTimerText(TextMeshProUGUI text)
    {
        int minutes = levelManager.gameTimer / 60;
        text.text = minutes.ToString("D2") + ":" + (levelManager.gameTimer % 60).ToString("D2");
    }
    void SetScoreText(TextMeshProUGUI text)
    {
        text.text = "Score: " + gameManager.score.ToString("D3");
    }
    void FirstSet()
    {
        livesText.text = gameManager.playerLives.ToString();
        SetTimerText(timerText);
        SetScoreText(scoreText);
    }
    IEnumerator SpawnCountdown()
    {
        //Deactivate HUD and activate timer
        HUD.SetActive(false);
        spawnTimerText.gameObject.SetActive(true);

        float respawnTimer = levelManager.playerRespawnTimer;

        //Update Timer
        do
        {
            spawnTimerText.text = ((int)respawnTimer + 1).ToString("D");
            respawnTimer -= Time.deltaTime;
            yield return null;
        } while (respawnTimer > 0);

        //Activate HUD and deactivate timer
        HUD.SetActive(true);
        spawnTimerText.gameObject.SetActive(false);

        yield break;
    }

    //Event Receivers
    void OnPlayerDied()
    {
        livesText.text = gameManager.playerLives.ToString();
        StartCoroutine(SpawnCountdown());
    }
    void OnPlayerWon()
    {
        HUD.SetActive(false);
        SetScoreText(victoryScoreText);
        SetTimerText(victoryTimerText);
        victoryPanel.SetActive(true);
    }
    void OnPlayerLost()
    {
        HUD.SetActive(false);
        SetScoreText(defeatScoreText);
        SetTimerText(defeatTimerText);
        defeatPanel.SetActive(true);
    }
    void OnScoreUpdated()
    {
        SetScoreText(scoreText);
    }
}