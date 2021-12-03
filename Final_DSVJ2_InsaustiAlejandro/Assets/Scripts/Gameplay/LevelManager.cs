using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public Action ScoreUpdated;
    public Action PlayerDied;
    public Action PlayerWon;
    public Action PlayerLost;
    public float playerRespawnTimer { get { return playerSpawnTimer; } }
    public int playerLives { get { return gameManager.playerLives; } }
    public int gameTimer { get { return (int)timer; } }
    [SerializeField] AreaManager playerAreaManager;
    [SerializeField] FrogController player;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] PickablesManager pickablesManager;
    [Header("Map")]
    [SerializeField] float mapLimitEnemyMod;
    [SerializeField] float mapLimit;
    [Header("Player")]
    [Tooltip("In minutes")]
    [SerializeField] float maxPlayTime;
    [SerializeField] float playerSpawnTimer;
    [SerializeField] int lifeScoreValue;
    [Header("Enemies")]
    [SerializeField] int spawnAreas;
    GameManager gameManager;
    bool gameOver;
    float timer;

    //Unity Events
    private void Awake()
    {
        //Link Actions
        enemyManager.EnemySpawned += OnEnemySpawned;
        player.Died += OnPlayerDied;
        playerAreaManager.PlayerReachedLastArea += OnPlayerWon;
        pickablesManager.ScoreChanged += OnScoreChanged;

        //Set map limits
        enemyManager.mapLimit = mapLimit * mapLimitEnemyMod;
        player.mapLimit = mapLimit;

        //Select first spawn area
        OnEnemySpawned();

        //Set Game Timer
        timer = maxPlayTime * 60;
    }
    private void Start()
    {
        gameManager = GameManager.Get();
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
    void RespawnPlayer()
    {
        player.gameObject.SetActive(true);
        player.Respawn();
    }

    //Event Receivers
    private void OnEnemySpawned()
    {
        //Send a random area (OPTIMICE LATER, USE PLAYER POSITION TO SELECT AREAS)
        enemyManager.publicSpawnArea = Random.Range(0, spawnAreas);
    }
    void OnPlayerDied()
    {
        player.gameObject.SetActive(false);

        if (playerLives > 1)
        {
            //Respawn player
            gameManager.playerLives--;
            PlayerDied.Invoke();
            Invoke("RespawnPlayer", playerSpawnTimer);

            //Set new player position
            Vector3 newSpawnPoint = player.transform.position;
            newSpawnPoint.z = playerAreaManager.GetHighestAreaPosition();
            player.transform.position = newSpawnPoint;
        }
        else
        {
            Debug.Log("Player Lost");
            gameOver = true;
            PlayerLost.Invoke();
        }
    }
    void OnPlayerWon()
    {
        Debug.Log("Player Won");
        gameOver = true;
        gameManager.score += (int)(gameManager.score * (timer / (maxPlayTime*60)));
        gameManager.score += gameManager.playerLives * lifeScoreValue;
        PlayerWon.Invoke();
    }
    void OnScoreChanged(int value)
    {
        gameManager.score += value;
        ScoreUpdated.Invoke();
    }
}