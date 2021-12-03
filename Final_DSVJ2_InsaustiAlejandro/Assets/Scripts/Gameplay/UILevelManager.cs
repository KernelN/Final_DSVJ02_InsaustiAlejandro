using UnityEngine;
using TMPro;
using System.Collections;

public class UILevelManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;
    [SerializeField] GameObject HUD;
    [SerializeField] TextMeshProUGUI spawnTimerText;
    [SerializeField] TextMeshProUGUI livesText;

    //Unity Events
    private void Start()
    {
        //Link Actions
        levelManager.PlayerDied += OnPlayerDied;
        levelManager.PlayerWon += OnPlayerWon;
        levelManager.PlayerLost += OnPlayerLost;

        //Set first values of evertything
        FirstSet();
    }
    private void LateUpdate()
    {
        
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
        victoryPanel.SetActive(true);
    }
    void OnPlayerLost()
    {
        defeatPanel.SetActive(true);
    }
}