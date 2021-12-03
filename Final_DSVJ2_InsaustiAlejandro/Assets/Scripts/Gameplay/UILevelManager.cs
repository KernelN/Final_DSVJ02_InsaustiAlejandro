using UnityEngine;
using TMPro;
using System.Collections;
using System;

public class UILevelManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;
    [SerializeField] GameObject HUD;
    [SerializeField] TextMeshProUGUI spawnTimerText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI scoreText;

    //Unity Events
    private void Start()
    {
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
        int minutes = levelManager.publicGameTimer / 60;
        timerText.text = minutes.ToString("D2") + ":" + (levelManager.publicGameTimer % 60).ToString("D2");
    }

    //Methods
    void FirstSet()
    {
        livesText.text = levelManager.publicPlayerLives.ToString();
    }
    IEnumerator SpawnCountdown()
    {
        //Deactivate HUD and activate timer
        HUD.SetActive(false);
        spawnTimerText.gameObject.SetActive(true);

        float respawnTimer = levelManager.publicPlayerSpawnTimer;

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
        livesText.text = levelManager.publicPlayerLives.ToString();
        StartCoroutine(SpawnCountdown());
    }
    void OnPlayerWon()
    {
        HUD.SetActive(false);
        victoryPanel.SetActive(true);
    }
    void OnPlayerLost()
    {
        HUD.SetActive(false);
        defeatPanel.SetActive(true);
    }
    void OnScoreUpdated(int score)
    {
        scoreText.text = "Score: " + score.ToString("D3");
    }
}