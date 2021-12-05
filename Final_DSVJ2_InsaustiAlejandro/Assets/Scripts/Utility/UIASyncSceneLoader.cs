using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIASyncSceneLoader : MonoBehaviour
{
	[SerializeField] Slider loadProgressBar;
	[SerializeField] TextMeshProUGUI sceneToLoadName;
	GameManager gameManager;
	ASyncSceneLoader sceneLoader;

    //Unity Events
    private void Start()
    {
        //Get variables
        gameManager = GameManager.Get();
        sceneLoader = ASyncSceneLoader.Get();

        SetLoadName();
    }
    private void Update()
    {
        //Update load bar
        loadProgressBar.value = sceneLoader.loadingProgress;
    }

    //Methods
    void SetLoadName()
    {
        string sceneName = "ERROR";

        switch (gameManager.targetScene)
        {
            case SceneLoader.Scenes.menu:
                sceneName = "Main Menu";
                break;
            case SceneLoader.Scenes.highscores:
                sceneName = "Highscores";
                break;
            case SceneLoader.Scenes.credits:
                sceneName = "Credits";
                break;
            case SceneLoader.Scenes.level1:
                sceneName = "Level 1";
                break;
            case SceneLoader.Scenes.level2:
                sceneName = "Level 2";
                break;
            case SceneLoader.Scenes.level3:
                sceneName = "Level 3";
                break;
            case SceneLoader.Scenes.level4:
                sceneName = "Level 4";
                break;
            case SceneLoader.Scenes.level5:
                sceneName = "Level 5";
                break;
            default:
                break;
        }

        sceneToLoadName.text = "Loading " + sceneName + "...";
    }
}