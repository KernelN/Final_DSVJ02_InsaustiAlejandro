using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISplashScreenGenerator : MonoBehaviour
{
	[SerializeField] GameObject splashTemplate;
	[SerializeField] Transform canvas;
	[SerializeField] Sprite[] splashImages;
    [Tooltip("How many seconds the splash will stay at full transparency")]
	[SerializeField] float splashPeakDuration;
    [Tooltip("How many seconds the splash will need to fade in or out")]
	[SerializeField] float splashFadeDuration;
    [Tooltip("How many seconds there will be between splashes")]
    [SerializeField] float secondsBetweenSplash;
	List<Image> splashes;
    Image blackScreen;

    //Unity Events
    private void Start()
    {
        if (!GameManager.Get().firstTimeOnMenu) return; //only make splash screens on first load

        //Create black background
        blackScreen = InstantiateSplash();

        //Get invisible color
        Color alphaZero = Color.white;
        alphaZero.a = 0;

        //Create Splash List
        splashes = new List<Image>();
        foreach (var image in splashImages)
        {
            splashes.Add(InstantiateSplash(image));
            splashes[splashes.Count - 1].color = alphaZero;
        }

        StartCoroutine(RunSplashImages());
    }

    //Methods
    void DestroyAll()
    {
        foreach (var image in splashes)
        {
            Destroy(image.gameObject);
        }
        StartCoroutine(DestroyBlackscreen());
    }
    Image InstantiateSplash(Sprite image = null)
    {
        GameObject imageGO = Instantiate(splashTemplate, canvas);
        Image splash = imageGO.AddComponent<Image>();
        if (image)
        {
            splash.sprite = image;
            imageGO.name = image.name;
            splash.preserveAspect = true;
        }
        else
        {
            splash.color = Color.black;
            imageGO.name = "Black Screen";
            splash.preserveAspect = false;
        }
        return splash;
    }
    IEnumerator RunSplashImages()
    {
        //Set Colors 
        Color splashColor;

        //Set timer
        float timer = 0;
        
        //Wait a little before load;
        yield return new WaitForSeconds(splashPeakDuration);

        //Make every splash appear and dissapear
        for (int i = 0; i < splashes.Count; i++)
        {
            splashColor = splashes[i].color;

            //Make splash fade in
            do
            {
                timer += Time.deltaTime;
                splashColor.a = timer / splashFadeDuration;
                splashes[i].color = splashColor;
                yield return null;
            } while (timer < splashFadeDuration);

            //Set timer to max and wait a little before fade out
            timer = splashFadeDuration;
            yield return new WaitForSeconds(splashPeakDuration);

            //Fade out
            do
            {
                timer -= Time.deltaTime;
                splashColor.a = timer / splashFadeDuration;
                splashes[i].color = splashColor; 
                yield return null;
            } while (timer > 0);

            //Wait before new splash appears
            yield return new WaitForSeconds(secondsBetweenSplash);
        }

        //Destroy all images
        DestroyAll();
        yield break;
    }
    IEnumerator DestroyBlackscreen()
    {
        //Set Colors 
        Color alphaColor = Color.black;

        //Set timer
        float timer = splashFadeDuration;

        //Fade out
        do
        {
            timer -= Time.deltaTime;

            alphaColor.a = timer / splashFadeDuration;
            blackScreen.color = alphaColor;
            yield return null;
        } while (timer > 0);

        Destroy(blackScreen.gameObject);
        yield break;
    }
}