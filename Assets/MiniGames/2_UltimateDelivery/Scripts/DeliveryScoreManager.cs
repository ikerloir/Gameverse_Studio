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
    }

    public void AddPoint()
    {
        score++;
        UpdateScoreDisplay();
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
        return score;
    }
}