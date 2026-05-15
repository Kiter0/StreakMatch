using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }
    public int Combo { get; private set; }
    public int HighScore { get; private set; }

    public void ResetScore()
    {
        Score = 0;
        Combo = 0;
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void StartCombo()
    {
        Combo = 1;
    }

    public void IncreaseCombo()
    {
        Combo++;
    }

    public void ResetCombo()
    {
        Combo = 0;
    }

    public void AddScore(int amount)
    {
        int finalScore = amount * Mathf.Max(Combo, 1);
        Score += finalScore;

        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.Save();
        }

        Debug.Log("Score: " + Score + " | Combo x" + Combo);
    }
}