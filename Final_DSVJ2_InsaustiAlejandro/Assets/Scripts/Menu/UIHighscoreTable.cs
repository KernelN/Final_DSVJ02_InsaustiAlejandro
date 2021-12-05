using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIHighscoreTable : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI highscoreTexts;
    const string divider = " - ";

    //Unity Events
    private void Start()
    {
        //Get Game Manager
        List<PlayerData> highscores = GameManager.Get().highscores;
        highscores.Sort(HighscoreSorter.Compare);
        highscoreTexts.text = "";

        for (int i = 0; i < highscores.Count; i++)
        {
            AddScoreToTable(highscores[i], i + 1);
        }
    }

    //Methods
    void AddScoreToTable(PlayerData data, int number)
    {
        string name = data.name;
        string score = data.score.ToString();
        string tablePos = number.ToString();

        if (number != 1)
        {
            highscoreTexts.text += "\n";
        }

        highscoreTexts.text += "#" + tablePos + " " + name + divider + score;
    }
}