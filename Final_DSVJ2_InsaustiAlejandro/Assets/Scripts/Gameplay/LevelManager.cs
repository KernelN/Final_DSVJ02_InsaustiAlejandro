using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviourSingletonInScene<LevelManager>
{
    public Action ScoreUpdated;
    public Action PlayerDied;
    public Action PlayerWon;
    public Action PlayerLost;
    //Map
    public float mapLimits { get { return mapLimit; } }
    //Player
    public float playerRespawnTimer { get { return playerSpawnTimer; } }
    public float playerCurrentCheckpoint 
    { get { return checkpointManager.GetCurrentCheckpointPosition(); } }
    //Enemies
    public int gameTimer { get { return (int)timer; } }
    [Header("Map")]
    [SerializeField] int level;
    [SerializeField] float mapLimit;
    [Header("Player")]
    [SerializeField] FrogController player;
    [SerializeField] CheckpointManager checkpointManager;
    [Tooltip("In minutes")]
    [SerializeField] float maxPlayTime;
    [SerializeField] float playerSpawnTimer;
    [SerializeField] int lifeScoreValue;
    GameManager gameManager;
    bool gameOver;
    float timer;

    //Unity Events
    private void Start()
    {
        //Link Actions
        player.Died += OnPlayerDied;
        checkpointManager.PlayerReachedLastArea += OnPlayerWon;

        //Set map limits
        player.mapLimit = mapLimit;

        //Set Game Timer
        timer = maxPlayTime * 60;

        gameManager = GameManager.Get();
        gameManager.level = level;
    }
    private void Update()
    {
        if (gameOver) return;

        //if timer less than 0, set it as 0
        if (timer <= 0)
        {
            timer = 0;
            gameOver = true;
            PlayerLost.Invoke();

            return;
        }

        //reduce timer
        timer -= Time.deltaTime;
    }

    //Methods
    public void AddScore(int value)
    {
        gameManager.score += value;
        ScoreUpdated.Invoke();
    }
    void RespawnPlayer()
    {
        player.gameObject.SetActive(true);
        player.Respawn();
    }

    //Event Receivers
    void OnPlayerDied()
    {
        player.gameObject.SetActive(false);

        if (gameManager.playerLives > 1)
        {
            //Respawn player
            gameManager.playerLives--;
            PlayerDied.Invoke();
            Invoke("RespawnPlayer", playerSpawnTimer);

            //Set new player position
            Vector3 newSpawnPoint = player.transform.position;
            newSpawnPoint.z = checkpointManager.GetHighestCheckpointPosition();
            player.transform.position = newSpawnPoint;
        }
        else
        {
            Debug.Log("Player Lost");
            gameOver = true;
            gameManager.level = 1;
            //multiply score by current game speed
            gameManager.score = (int)(gameManager.score * Time.deltaTime); 
            PlayerLost.Invoke();
            gameManager.SetPause(0);
        }
    }
    void OnPlayerWon()
    {
        Debug.Log("Player Won");
        gameOver = true;
        gameManager.score += (int)(gameManager.score * (timer / (maxPlayTime*60)));
        gameManager.score += gameManager.playerLives * lifeScoreValue;
        gameManager.level++;
        PlayerWon.Invoke();
        gameManager.SetPause(0);
    }
}