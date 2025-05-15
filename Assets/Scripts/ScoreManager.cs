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
            Debug.Log("ScoreManager initialized as singleton");
        }
        else
        {
            Destroy(gameObject);
        }

        gameScores = new int[4];
        Debug.Log("ScoreManager: Array de puntuaciones inicializado con 4 elementos");
    }

    public void SetScore(int gameIndex, int score)
    {
        if (gameIndex < 0 || gameIndex >= gameScores.Length)
        {
            Debug.LogError($"ScoreManager: Índice de juego inválido: {gameIndex}. Debe estar entre 0 y {gameScores.Length - 1}");
            return;
        }

        int oldScore = gameScores[gameIndex];
        gameScores[gameIndex] = Mathf.Clamp(score, 0, maxScorePerGame);

        string gameName = GetGameName(gameIndex);
        string oldMedal = GetMedalType(oldScore);
        string newMedal = GetMedalType(gameScores[gameIndex]);

        Debug.Log($"ScoreManager: Juego {gameName} - Puntuación actualizada de {oldScore} ({oldMedal}) a {gameScores[gameIndex]} ({newMedal})");
        Debug.Log($"ScoreManager: Estado actual de puntuaciones: {string.Join(", ", gameScores)}");
    }

    public int GetScore(int gameIndex)
    {
        if (gameIndex < 0 || gameIndex >= gameScores.Length)
        {
            Debug.LogError($"ScoreManager: Índice de juego inválido: {gameIndex}. Debe estar entre 0 y {gameScores.Length - 1}");
            return 0;
        }
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
        float starRating = Mathf.Round((totalScore / (float)maxTotalScore) * 5f * 10f) / 10f;

        Debug.Log($"ScoreManager: Cálculo de estrellas finales:");
        Debug.Log($"ScoreManager: Puntuación total: {totalScore} de {maxTotalScore} posible");
        Debug.Log($"ScoreManager: Rating de estrellas: {starRating}");

        return starRating;
    }

    // metodo que reinicia puntaciones si se vuelve a juegar
    public void ResetScores()
    {
        for (int i = 0; i < gameScores.Length; i++)
        {
            gameScores[i] = 0;
        }
        Debug.Log("ScoreManager: Todas las puntuaciones han sido reiniciadas a 0");
    }

    private string GetGameName(int gameIndex)
    {
        switch (gameIndex)
        {
            case 0: return "AirboneDanger";
            case 1: return "Ultimate Delivery";
            case 2: return "Mortal Bag";
            case 3: return "Ultimate Defense";
            default: return $"Juego {gameIndex}";
        }
    }

    private string GetMedalType(int score)
    {
        if (score >= 90) return "Triple Oro";
        if (score >= 70) return "Doble Oro";
        if (score >= 50) return "Oro";
        if (score >= 30) return "Plata";
        if (score >= 10) return "Bronce";
        return "Sin medalla";
    }
}
