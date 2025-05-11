using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int[] gameScores;
    //private Dictionary<GameManager.GameScenes, int> gameScores = new Dictionary<GameManager.GameScenes, int>();

    // Puedes personalizar esto por juego si lo necesitas
    private const int maxScorePerGame = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameScores = new int[3];
    }

    public void SetScore(int gameIndex, int score)
    {
        gameScores[gameIndex] = Mathf.Clamp(score, 0, maxScorePerGame);
    }

    public int GetScore(int gameIndex)
    {
        return gameScores[gameIndex];
    }

    public float CalculateFinalStarRating()
    {
        int totalScore = 0;

        foreach (int game in gameScores)
        {
            totalScore += game;
        }

        int maxTotalScore = gameScores.Length * maxScorePerGame;

        // Normalizamos a una escala de 0 a 5 estrellas
        return Mathf.Round((totalScore / (float)maxTotalScore) * 5f * 10f) / 10f;
    }
    // metodo que reinicia puntaciones si se vuelve a juegar
    public void ResetScores()
    {
        for (int i = 0; i < gameScores.Length; i++)
        {
            gameScores[i] = 0;
        }
    }

}
