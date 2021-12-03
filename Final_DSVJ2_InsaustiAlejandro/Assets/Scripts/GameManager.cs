using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
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
    public void LoadMenu()
    {
        currentScene = SceneLoader.Scenes.menu;
        SceneLoader.LoadScene(currentScene);
    }
    public void LoadLevel1()
    {
        currentScene = SceneLoader.Scenes.level1;
        SceneLoader.LoadScene(currentScene);
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