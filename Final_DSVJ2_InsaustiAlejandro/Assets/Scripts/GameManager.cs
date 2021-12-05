using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Serializable]
    class HighscoresTable { public List<PlayerData> table; }

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
    public List<PlayerData> highscores { get { return highscoreTable.table; } }
    public SceneLoader.Scenes targetScene { get { return currentScene; } }
    public bool firstTimeOnMenu = true;
    [SerializeField] PlayerData playerData;
    [SerializeField] HighscoresTable highscoreTable;
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
        highscoreTable.table.Add(playerData);
        highscoreTable.table.Sort(HighscoreSorter.Compare);
        DeleteScore();
    }
    public void DeleteScore()
    {
        playerData = templateData;
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
        //Get player data
        PlayerData temp = new PlayerData();
        playerData = JsonFileManager<PlayerData>.LoadDataFromFile(temp, Application.persistentDataPath + "/data.bin");
        if (playerData.level < 1) playerData = templateData; //if there is no player, load template
     
        //Get Highscores
        HighscoresTable tempList = new HighscoresTable();
        highscoreTable = JsonFileManager<HighscoresTable>.LoadDataFromFile(tempList, Application.persistentDataPath + "/highscores.bin");
        if (highscoreTable == null) highscoreTable = new HighscoresTable();
    }
    void SaveData()
    {
        JsonFileManager<PlayerData>.SaveDataToFile(playerData, Application.persistentDataPath + "/data.bin");
        JsonFileManager<HighscoresTable>.SaveDataToFile(highscoreTable, Application.persistentDataPath + "/highscores.bin");
    }
}