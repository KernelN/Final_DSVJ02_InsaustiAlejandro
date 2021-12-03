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
    public void LoadCredits()
    {
        gameManager.LoadCredits();
    }
    public void QuitGame()
    {
        gameManager.QuitGame();
    }
}