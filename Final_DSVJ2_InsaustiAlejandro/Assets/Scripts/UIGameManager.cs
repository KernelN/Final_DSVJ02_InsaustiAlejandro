using UnityEngine;

public class UIGameManager : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Get();
    }

    public void LoadMenu()
    {
        gameManager.LoadMenu();
    }
    public void LoadLevel1()
    {
        gameManager.LoadLevel1();
    }
    public void LoadLevel2()
    {
        gameManager.LoadLevel2();
    }
    public void LoadLevel3()
    {
        gameManager.LoadLevel3();
    }
    public void LoadLastLevel()
    {
        gameManager.LoadLastLevel();
    }
    public void LoadCredits()
    {
        gameManager.LoadCredits();
    }
    public void QuitGame()
    {
        gameManager.QuitGame();
    }
}