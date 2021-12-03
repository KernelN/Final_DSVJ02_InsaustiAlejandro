using UnityEngine;
using TMPro;

public class UILevelManager : MonoBehaviour
{
	[SerializeField] LevelManager levelManager;
	[SerializeField] GameObject victoryPanel;
	[SerializeField] GameObject defeatPanel;

    //Unity Events
    private void Start()
    {
        levelManager.PlayerWon += OnPlayerWon;
        levelManager.PlayerLost += OnPlayerLost;
    }

    //Event Receivers
    void OnPlayerWon()
    {
        victoryPanel.SetActive(true);
    }
    void OnPlayerLost()
    {
        defeatPanel.SetActive(true);
    }
}