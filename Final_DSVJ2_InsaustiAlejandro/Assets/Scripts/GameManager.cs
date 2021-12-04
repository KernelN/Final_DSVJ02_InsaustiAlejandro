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
    public SceneLoader.Scenes targetScene { get { return currentScene; }  }
    [SerializeField] PlayerData playerData;
    [SerializeField] int maxLevel;
    SceneLoader.Scenes currentScene;

    //Unity Events
    private void Start()
    {
        currentScene = SceneLoader.GetCurrentScene();
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
    public void LoadMenu()
    {
        if(Time.timeScale != 1) SetPause(1);

        currentScene = SceneLoader.Scenes.menu;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel1()
    {
        currentScene = SceneLoader.Scenes.level1;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel2()
    {
        currentScene = SceneLoader.Scenes.level2;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel3()
    {
        currentScene = SceneLoader.Scenes.level3;
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
            default:
                break;
        }
    }
    public void LoadCredits()
    {
        currentScene = SceneLoader.Scenes.credits;
        SceneLoader.LoadScene(currentScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}