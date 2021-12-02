using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] PlayerController player;
	[SerializeField] EnemyManager enemyManager;
	[SerializeField] float mapLength;
	[SerializeField] int spawnArea;

    //Unity Events
    private void Awake()
    {
        enemyManager.mapLimit = mapLength * 2;
        player.mapLimit = mapLength*2;
    }
    private void Update()
    {
        enemyManager.publicSpawnArea = spawnArea - 1;
    }
}