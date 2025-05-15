using UnityEngine;
using TMPro;

public class MortalBagScoreManager : MonoBehaviour
{
    public static MortalBagScoreManager Instance;
    public TextMeshProUGUI scoreText;
    private int score = 0;

    void Awake()
    {
        Debug.Log("MortalBagScoreManager Awake");
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MortalBagScoreManager inicializado como singleton");
        }
        else
        {
            Debug.Log("Ya existe una instancia de MortalBagScoreManager, destruyendo duplicado");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("MortalBagScoreManager Start");
        if (scoreText == null)
        {
            Debug.LogError("¡Error! El TextMeshProUGUI no está asignado en el MortalBagScoreManager");
        }
        else
        {
            Debug.Log("TextMeshProUGUI asignado correctamente");
        }
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        Debug.Log($"AddScore llamado con {points} puntos");
        score += points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
            Debug.Log($"Score actualizado a: {score}");
        }
        else
        {
            Debug.LogError("No se puede actualizar el texto porque scoreText es null");
        }
    }

    public int GetScore()
    {
        return score;
    }
}
