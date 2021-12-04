using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformManager : MonoBehaviour
{
    public Action PlatformSpawned;
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
    [SerializeField] GameObject platformTemplate;
    [SerializeField] float spawnCooldown;
    [SerializeField] float spawnChance;
    [SerializeField] int maxPlatforms;
    List<PlatformController> enabledPlatforms;
    List<PlatformController> disabledPlatforms;
    LevelManager levelManager;
    float spawnTimer;
    int spawnArea;

    //Unity Events
    private void Start()
    {
        //Get LevelManger
        //levelManager = LevelManager.Get();

        //Generate lists
        enabledPlatforms = new List<PlatformController>();
        disabledPlatforms = new List<PlatformController>();

        //Generate Enemies
        GameObject enemy;
        for (int i = 0; i < maxPlatforms; i++)
        {
            //Set Gameobject
            enemy = Instantiate(platformTemplate, transform);
            enemy.name = platformTemplate.name + " " + (i + 1);
            enemy.SetActive(false);

            disabledPlatforms.Add(enemy.GetComponent<PlatformController>());
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

        //Set Position and Rotation
        platform.transform.position = spawnPoints[spawnArea].position;
        platform.transform.rotation = spawnPoints[spawnArea].rotation;

        //Activate
        platform.gameObject.SetActive(true);

        //Call action
        PlatformSpawned.Invoke();
    }

    //Event Receivers
    void OnPlatformDeath(PlatformController PlatformEnemy)
    {
        //Switch Pools
        enabledPlatforms.Remove(PlatformEnemy);
        disabledPlatforms.Add(PlatformEnemy);

        PlatformEnemy.gameObject.SetActive(false);
    }
}