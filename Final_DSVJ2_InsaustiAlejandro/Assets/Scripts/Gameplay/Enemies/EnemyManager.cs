using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public float mapLimit;
    public int publicSpawnArea 
    {
        private get { return spawnArea; }
        set
        {
            spawnArea = value;

            if (spawnArea > spawnPoints.Length)
                spawnArea = spawnPoints.Length - 1;
            else if (spawnArea < 0)
                spawnArea = 0;
        }
    }
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject enemyTemplate;
    [SerializeField] Transform enemyEmpty;
    [SerializeField] float spawnCooldown;
    [SerializeField] float spawnChance;
    [SerializeField] int maxEnemies;
    List<EnemyController> enabledEnemies;
    List<EnemyController> disabledEnemies;
    float spawnTimer;
    int spawnArea;

    //Unity Events
    private void Start()
    {
        enabledEnemies = new List<EnemyController>();
        disabledEnemies = new List<EnemyController>();

        GameObject enemy;
        for (int i = 0; i < maxEnemies; i++)
        {
            //Set Gameobject
            enemy = Instantiate(enemyTemplate, enemyEmpty);
            enemy.name = enemyTemplate.name + " " + (i + 1);
            enemy.SetActive(false);

            disabledEnemies.Add(enemy.GetComponent<EnemyController>());
            disabledEnemies[i].mapLimit = mapLimit;
            disabledEnemies[i].Died = OnEnemyDeath;
        }
    }
    private void Update()
    {
        //Increase timer until it grows bigger than max
        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnCooldown) return;

        //Reset Timer
        spawnTimer = 0;

        if (Random.Range(0, 100) < spawnChance) //if inside chance range, spawn enemy
        {
            SpawnEnemy();
        }
    }

    //Methods
    void SpawnEnemy()
    {
        if (disabledEnemies.Count < 1) return;

        //Switch Pools
        EnemyController enemy = disabledEnemies[0];
        enabledEnemies.Add(enemy);
        disabledEnemies.Remove(enemy);

        //Set Position and Rotation
        enemy.transform.position = spawnPoints[spawnArea].position;
        enemy.transform.rotation = spawnPoints[spawnArea].rotation;

        //Activate
        enemy.gameObject.SetActive(true);
    }

    //Event Receivers
    void OnEnemyDeath(EnemyController deadEnemy)
    {
        //Switch Pools
        enabledEnemies.Remove(deadEnemy);
        disabledEnemies.Add(deadEnemy);

        deadEnemy.gameObject.SetActive(false);
    }
}