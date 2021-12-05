﻿using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public int playerLives
    {
        get { return playerData.playerLives; }
        set { if (value < 0) value = 0; playerData.playerLives = value; }
    }
    public int score
    {
        get { return playerData.score; }
        set { if (value < 0) value = 0; playerData.score = value; }
    }
    public int level //max values changes with max level
    {
        get { return playerData.level; }
        set { if (value < 1) value = 1; if (value > maxLevel) value = 1; playerData.level = value; }
    }
    public List<PlayerData> highscores { get { return highscoreTable; } }
    public SceneLoader.Scenes targetScene { get { return currentScene; } }
    public bool firstTimeOnMenu = true;
    [SerializeField] PlayerData playerData;
    [SerializeField] List<PlayerData> highscoreTable;
    [SerializeField] int maxLevel;
    SceneLoader.Scenes currentScene;
    PlayerData templateData;

    //Unity Events
    private void Start()
    {
        templateData = playerData;
        currentScene = SceneLoader.GetCurrentScene();
        ReceiveData();
    }
    private void OnDestroy()
    {
        if (GameManager.Get() == this)
        {
            QuitGame();
        }
    }

    //Methods
    public void SetPause(float newTime)
    {
        Time.timeScale = newTime;
    }
    public void AddScoreToHighscore(string name)
    {
        playerData.name = name;
        highscoreTable.Add(playerData);
        highscoreTable.Sort(HighscoreSorter.Compare);
        DeleteScore();
    }
    public void DeleteScore()
    {
        playerData = new PlayerData();
    }
    public void LoadMenu()
    {
        if(Time.timeScale != 1) SetPause(1);

        currentScene = SceneLoader.Scenes.menu;
        SceneLoader.LoadScene(currentScene);
        firstTimeOnMenu = false;
    }
    public void LoadLevel1()
    {
        if (Time.timeScale != 1) SetPause(1);

        currentScene = SceneLoader.Scenes.level1;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel2()
    {
        if (Time.timeScale != 1) SetPause(1);

        currentScene = SceneLoader.Scenes.level2;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel3()
    {
        if (Time.timeScale != 1) SetPause(1);

        currentScene = SceneLoader.Scenes.level3;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel4()
    {
        if (Time.timeScale != 1) SetPause(1);

        currentScene = SceneLoader.Scenes.level4;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel5()
    {
        if (Time.timeScale != 1) SetPause(1);

        currentScene = SceneLoader.Scenes.level5;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLastLevel()
    {
        switch (level)
        {
            case 1:
                LoadLevel1();
                break;
            case 2:
                LoadLevel2();
                break;
            case 3:
                LoadLevel3();
                break;
            case 4:
                LoadLevel4();
                break;
            case 5:
                LoadLevel5();
                break;
            default:
                break;
        }
    }
    public void LoadHighscores()
    {
        currentScene = SceneLoader.Scenes.highscores;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadCredits()
    {
        currentScene = SceneLoader.Scenes.credits;
        SceneLoader.LoadScene(currentScene);
    }
    public void QuitGame()
    {
        SaveData();
        Application.Quit();
    }
    void ReceiveData()
    {
        List<PlayerData> temp = new List<PlayerData>(1);
        playerData = JsonFileManager<PlayerData>.LoadDataFromFile(temp[0], Application.persistentDataPath + "data.bin");
        highscoreTable = JsonFileManager<List<PlayerData>>.LoadDataFromFile(temp, Application.persistentDataPath + "data.bin");
    }
    void SaveData()
    {
        JsonFileManager<PlayerData>.SaveDataToFile(playerData, Application.persistentDataPath + "data.bin");
        JsonFileManager<List<PlayerData>>.SaveDataToFile(highscoreTable, Application.persistentDataPath + "data.bin");
    }
}