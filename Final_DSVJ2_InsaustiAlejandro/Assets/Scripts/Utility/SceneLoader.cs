using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static AsyncOperation sceneLoading;

    public enum Scenes { menu, credits, level1, level2, level3, level4, level5 }
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
        SceneManager.LoadScene("Load Scene");

        string sceneName = "Menu";
        switch (sceneToLoad)
        {
            case Scenes.menu:
                sceneName = "Menu";
                break;
            case Scenes.credits:
                //sceneName = "Credits";
                break;
            case Scenes.level1:
                sceneName = "Level 1";
                break;
            case Scenes.level2:
                sceneName = "Level 2";
                break;
            case Scenes.level3:
                sceneName = "Level 3";
                break;
            case Scenes.level4:
                sceneName = "Level 4";
                break;
            case Scenes.level5:
                sceneName = "Level 5";
                break;
            default:
                break;
        }

        sceneLoading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
     
        //Prevent scene from activating before loading screen is over
        sceneLoading.allowSceneActivation = false;
    }
}