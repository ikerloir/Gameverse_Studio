using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Score Settings")]
    [SerializeField] private int initialScore = 0;

    private int score;

    private void Start()
    {
        score = initialScore;
        UpdateScoreDisplay();
        Debug.Log($"DeliveryScoreManager started with score: {score}");
    }

    public void AddPoint()
    {
        score++;
        UpdateScoreDisplay();
        Debug.Log($"Score increased to: {score}");
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
        else
        {
            Debug.LogWarning("Score Text reference is missing in DeliveryScoreManager!");
        }
    }

    public int GetScore()
    {
        Debug.Log($"GetScore called, returning: {score}");
        return score;
    }

    public void PassScoreToGeneralManager()
    {
        if (ScoreManager.Instance != null)
        {
            int scaledScore = score * 10; // Convertir a escala 0-100
            ScoreManager.Instance.SetScore(1, scaledScore); // Índice 1 para Ultimate Delivery
            Debug.Log($"Passing scaled score to general manager: {scaledScore}");
        }
        else
        {
            Debug.LogError("ScoreManager.Instance is null! Make sure ScoreManager is in a DontDestroyOnLoad scene.");
        }
    }

    public string GetMedalType()
    {
        string medalType;
        if (score >= 9)
            medalType = "TripleGold";
        else if (score >= 7)
            medalType = "DoubleGold";
        else if (score >= 5)
            medalType = "Gold";
        else if (score >= 3)
            medalType = "Silver";
        else if (score >= 0)
            medalType = "Bronze";
        else
            medalType = "None";

        Debug.Log($"GetMedalType called with score {score}, returning: {medalType}");
        return medalType;
    }
}