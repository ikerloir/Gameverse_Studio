using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private Dictionary<GameManager.GameScenes, int> gameScores = new Dictionary<GameManager.GameScenes, int>();

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
    }

    public void SetScore(GameManager.GameScenes game, int score)
    {
        gameScores[game] = Mathf.Clamp(score, 0, maxScorePerGame);
    }

    public int GetScore(GameManager.GameScenes game)
    {
        return gameScores.ContainsKey(game) ? gameScores[game] : 0;
    }

    public float CalculateFinalStarRating(GameManager.GameScenes[] gamesToEvaluate)
    {
        int totalScore = 0;

        foreach (var game in gamesToEvaluate)
        {
            totalScore += GetScore(game);
        }

        int maxTotalScore = gamesToEvaluate.Length * maxScorePerGame;

        // Normalizamos a una escala de 0 a 5 estrellas
        return Mathf.Round((totalScore / (float)maxTotalScore) * 5f * 10f) / 10f;
    }
}
