using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject platformTemplate;
    [Tooltip("Unity length to the player needed to enable a spawnpoint")]
    [SerializeField] float spawnMinDistance = 11;
    [SerializeField] float spawnCooldown;
    [SerializeField] float spawnChance;
    [SerializeField] float mapLimitMod;
    [SerializeField] int maxPlatforms;
    List<PlatformController> enabledPlatforms;
    List<PlatformController> disabledPlatforms;
    LevelManager levelManager;
    float mapLimit { get { return levelManager.mapLimits * mapLimitMod; } }
    float spawnTimer;

    //Unity Events
    private void Start()
    {
        //Get Level Manager
        levelManager = LevelManager.Get();

        //Generate lists
        enabledPlatforms = new List<PlatformController>();
        disabledPlatforms = new List<PlatformController>();

        //Generate Enemies
        GameObject platform;
        for (int i = 0; i < maxPlatforms; i++)
        {
            //Set Gameobject
            platform = Instantiate(platformTemplate, transform);
            platform.name = platformTemplate.name + " " + (i + 1);
            platform.SetActive(false);

            disabledPlatforms.Add(platform.GetComponent<PlatformController>());
            disabledPlatforms[i].mapLimit = mapLimit;
            disabledPlatforms[i].Died = OnPlatformDeath;
        }

        Vector3 spawnPos;
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPos = spawnPoint.transform.position; //get position

            //Generate new position
            if (spawnPos.x < 0)
            {
                spawnPos.x = -mapLimit;
            }
            else
            {
                spawnPos.x = mapLimit;
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
            SpawnPlatform();
        }
    }

    //Methods    
    void SpawnPlatform()
    {
        if (disabledPlatforms.Count < 1) return;

        //Switch Pools
        PlatformController platform = disabledPlatforms[0];
        enabledPlatforms.Add(platform);
        disabledPlatforms.Remove(platform);

        //Get spawn areas
        Vector2 spawnAreas = GetSpawnAreas();
        int minSpawnArea = (int)spawnAreas.x;
        int maxSpawnArea = (int)spawnAreas.y;
        int spawnArea = Random.Range(minSpawnArea, maxSpawnArea);

        //Set Position and Rotation
        platform.transform.position = spawnPoints[spawnArea].position;
        platform.transform.rotation = spawnPoints[spawnArea].rotation;

        //Activate
        platform.gameObject.SetActive(true);
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
    void OnPlatformDeath(PlatformController deadPlatform)
    {
        //Switch Pools
        enabledPlatforms.Remove(deadPlatform);
        disabledPlatforms.Add(deadPlatform);

        deadPlatform.gameObject.SetActive(false);
    }
}