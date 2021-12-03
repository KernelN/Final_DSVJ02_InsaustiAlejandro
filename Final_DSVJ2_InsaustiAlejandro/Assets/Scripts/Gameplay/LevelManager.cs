using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] AreaManager playerAreaManager;
	[SerializeField] PlayerController player;
	[SerializeField] EnemyManager enemyManager;
	[SerializeField] CameraController cameraController;
	[SerializeField] float mapLimitEnemyMod;
	[SerializeField] float mapLimit;
	[SerializeField] float playerSpawnTimer;
	[SerializeField] int playerLives;
	[SerializeField] int spawnAreas;

    //Unity Events
    private void Awake()
    {
        //Link Actions
        enemyManager.EnemySpawned += OnEnemySpawned;
        player.Died += OnPlayerDied;
        playerAreaManager.PlayerReachedLastArea += OnPlayerWon;

        //Set map limits
        enemyManager.mapLimit = mapLimit * mapLimitEnemyMod;
        player.mapLimit = mapLimit;

        //Select first spawn area
        OnEnemySpawned();
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

        if (playerLives > 0)
        {
            //Respawn player
            playerLives--;
            Invoke("RespawnPlayer", playerSpawnTimer);

            //Set new player position
            Vector3 newSpawnPoint = player.transform.position;
            newSpawnPoint.z = playerAreaManager.GetHighestAreaPosition();
            player.transform.position = newSpawnPoint;

            //Set camera
            cameraController.GoToMinDistance();
        }
    }
    void OnPlayerWon()
    {
        Debug.Log("Player Won");
    }
}