using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] PlayerController player;
	[SerializeField] EnemyManager enemyManager;
	[SerializeField] float mapLimitEnemyMod;
	[SerializeField] float mapLimit;
	[SerializeField] int spawnAreas;

    //Unity Events
    private void Awake()
    {
        //Link Actions
        enemyManager.EnemySpawned += OnEnemySpawned;

        //Set map limits
        enemyManager.mapLimit = mapLimit * mapLimitEnemyMod;
        player.mapLimit = mapLimit;

        //Select first spawn area
        OnEnemySpawned();
    }

    //Event Receivers
    private void OnEnemySpawned()
    {
        //Send a random area (OPTIMICE LATER, USE PLAYER POSITION TO SELECT AREAS)
        enemyManager.publicSpawnArea = Random.Range(0, spawnAreas);
    }
}