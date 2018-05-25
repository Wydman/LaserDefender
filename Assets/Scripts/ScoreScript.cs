using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

    Text textScore;
    string scoreString;
    public static int score = 0;
    public bool playerIsAlive = true;

    private void Start()
    {
        textScore = GetComponent<Text>();
        scoreString = (" Score " + score);
        textScore.text = scoreString.ToString();
    }
    public void ScoreCalcul(int points)
    {
        if (playerIsAlive)
        {
            score += points;
            scoreString = (" Score " + score);
            textScore.text = scoreString.ToString();
        }
    }

    /*
    public void IncrementationLevelNumber()
    {
        LevelManager.levelNumber++;
    }
    */
}
