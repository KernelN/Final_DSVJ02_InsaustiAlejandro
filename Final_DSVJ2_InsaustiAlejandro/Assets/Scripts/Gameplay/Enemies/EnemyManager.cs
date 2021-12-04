using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject enemyTemplate;
    [Tooltip("Unity length to the player needed to enable a spawnpoint")]
    [SerializeField] float spawnMinDistance = 11;
    [SerializeField] float spawnCooldown;
    [SerializeField] float spawnChance;
    [SerializeField] float mapLimitMod;
    [SerializeField] int maxEnemies;
    List<EnemyController> enabledEnemies;
    List<EnemyController> disabledEnemies;
    LevelManager levelManager;
	float mapLimit { get { return levelManager.mapLimits * mapLimitMod; } }
    float spawnTimer;

    //Unity Events
    private void Start()
    {
        //Get Level Manager
        levelManager = LevelManager.Get();

        //Generate lists
        enabledEnemies = new List<EnemyController>();
        disabledEnemies = new List<EnemyController>();

        //Generate Enemies
        GameObject enemy;
        for (int i = 0; i < maxEnemies; i++)
        {
            //Set Gameobject
            enemy = Instantiate(enemyTemplate, transform);
            enemy.name = enemyTemplate.name + " " + (i + 1);
            enemy.SetActive(false);

            disabledEnemies.Add(enemy.GetComponent<EnemyController>());
            disabledEnemies[i].mapLimit = mapLimit;
            disabledEnemies[i].Died = OnEnemyDeath;
        }

        Vector3 spawnPos;
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPos = spawnPoint.transform.position; //get position

            //Generate new position
            if (spawnPos.x < 0)
            {
                spawnPos.x = -mapLimit + enemyTemplate.transform.localScale.z / 2;
            }
            else
            {
                spawnPos.x = mapLimit - enemyTemplate.transform.localScale.z / 2;
            }

            //Set position
            spawnPoint.transform.position = spawnPos;
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

        //Get spawn areas
        Vector2 spawnAreas = GetSpawnAreas();
        int minSpawnArea = (int)spawnAreas.x;
        int maxSpawnArea = (int)spawnAreas.y;
        int spawnArea = Random.Range(minSpawnArea, maxSpawnArea);

        //Set Position and Rotation
        enemy.transform.position = spawnPoints[spawnArea].position;
        enemy.transform.rotation = spawnPoints[spawnArea].rotation;

        //Activate
        enemy.gameObject.SetActive(true);
    }
    Vector2 GetSpawnAreas()
    {
        //Init min and max range
        int minSpawn = 0;
        int maxSpawn = spawnPoints.Length - 1;

        for (int i = minSpawn; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].position.z < levelManager.playerCurrentCheckpoint - spawnMinDistance)
                break;
            minSpawn = i;
        }

        for (int i = maxSpawn; i > -1; i--)
        {
            if (spawnPoints[i].position.z > levelManager.playerCurrentCheckpoint + spawnMinDistance)
                break;
            maxSpawn = i;
        }

        //Return range
        return new Vector2(minSpawn, maxSpawn);
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