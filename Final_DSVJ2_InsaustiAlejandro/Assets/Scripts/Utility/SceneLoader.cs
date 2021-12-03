using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes { menu, credits, level1 }
    public static Scenes GetCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        switch (currentScene.name)
        {
            case "Main Menu":
                return Scenes.menu;
            case "Credits":
                return Scenes.credits;
            case "Level 1":
                return Scenes.level1;
            default:
                return Scenes.menu;
        }
    }
    public static void LoadScene(Scenes sceneToLoad)
    {
        switch (sceneToLoad)
        {
            case Scenes.menu:
        SceneManager.LoadScene("Main Menu");
                break;
            case Scenes.credits:
                //SceneManager.LoadScene("Credits");
                break;
            case Scenes.level1:
                SceneManager.LoadScene("Level 1");
                break;
            default:
                break;
        }
    }
}