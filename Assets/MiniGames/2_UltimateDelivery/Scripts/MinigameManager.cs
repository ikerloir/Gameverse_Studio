using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Canvas introCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas outroCanvas;
    [SerializeField] private GameObject player;
    [SerializeField] private float gameTime = 180f; // 3 minutes in seconds
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private HealthBarSlider healthBar;

    [Header("Outro Canvas Elements")]
    [SerializeField] private Image medalImage;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Sprite bronzeMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite doubleGoldMedal;
    [SerializeField] private Sprite tripleGoldMedal;

    private float currentTime;
    private bool isGameActive = false;
    private DeliveryScoreManager scoreManager;
    private OutroManager outroManager;

    void Start()
    {
        introCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        outroCanvas.gameObject.SetActive(false);

        if (player != null)
        {
            player.SetActive(false);
        }

        scoreManager = FindObjectOfType<DeliveryScoreManager>();
        outroManager = FindObjectOfType<OutroManager>();
    }

    public void StartMinigame()
    {
        introCanvas.gameObject.SetActive(false);
        outroCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);

        if (player != null)
        {
            player.SetActive(true);
        }

        currentTime = gameTime;
        isGameActive = true;
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (currentTime > 0 && isGameActive)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }

        // If time runs out and player still has health, end the game
        if (currentTime <= 0 && isGameActive)
        {
            GameOver();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        // Disable game canvas and show outro canvas
        gameCanvas.gameObject.SetActive(false);

        // Show outro using OutroManager
        if (outroManager != null)
        {
            outroManager.ShowOutro();
        }
        else
        {
            Debug.LogError("OutroManager not found!");
        }

        // Disable player
        if (player != null)
        {
            player.SetActive(false);
        }

        // Log final score
        if (scoreManager != null)
        {
            Debug.Log($"Game Over! Final Score: {scoreManager.GetScore()}, Medal Type: {scoreManager.GetMedalType()}");
        }
    }

    public void ReturnToMainGame()
    {
        // Connect to the "Back" button in the main game.        
    }

    public void ResetMinigame()
    {
        introCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        outroCanvas.gameObject.SetActive(false);

        if (player != null)
        {
            player.SetActive(false);
        }
    }
}