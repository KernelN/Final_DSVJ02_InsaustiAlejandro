using UnityEngine;
using TMPro;

public class UIHighscoreSubmitter : MonoBehaviour
{
	[SerializeField] TMP_InputField playerName;
	GameManager gameManager;

    //Unity Events
    private void Start()
    {
        gameManager = GameManager.Get();
    }

    //Methods
    public void AddScoreToHighscore()
    {
        if (playerName.text == "") return;

        gameManager.AddScoreToHighscore(playerName.text);
    }
    public void DeleteCurrentScore()
    {
        gameManager.DeleteScore();
    }
}