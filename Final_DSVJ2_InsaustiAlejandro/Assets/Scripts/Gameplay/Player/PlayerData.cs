using System;

[Serializable]
public class PlayerData
{
    public string name;
    public int playerLives;
    public int score;
    public int level;
}

public static class HighscoreSorter
{
    public static int Compare(PlayerData x, PlayerData y)
    {
        return y.score.CompareTo(x.score);
    }
}