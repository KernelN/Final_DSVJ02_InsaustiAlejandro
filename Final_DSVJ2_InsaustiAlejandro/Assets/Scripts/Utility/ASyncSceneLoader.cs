using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ASyncSceneLoader : MonoBehaviourSingletonInScene<ASyncSceneLoader>
{
    public float loadingProgress { get { return asyncLoad.progress; } }
    public bool sceneIsLoading { get { return !loadIsDone; } }
    [SerializeField] float minLoadSeconds;
    AsyncOperation asyncLoad;
    bool loadIsDone;

    //Unity Events
    private void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    //Methods
    IEnumerator LoadAsyncScene()
    {
        //The Application loads the Scene in the background as the current Scene runs.
        asyncLoad = SceneLoader.sceneLoading;

        //Set timer
        float timer = minLoadSeconds;

        // Wait until the asynchronous scene fully loads
        do
        {
            timer -= Time.deltaTime;
            yield return null;
        } while (asyncLoad.progress < 0.9f || timer > 0);

        loadIsDone = true;
        
        asyncLoad.allowSceneActivation = true;

        SceneManager.UnloadSceneAsync("Load Scene");
        yield break;
    }
}