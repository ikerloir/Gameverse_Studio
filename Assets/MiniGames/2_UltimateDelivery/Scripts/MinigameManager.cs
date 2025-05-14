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
    [SerializeField] private TextMeshProUGUI scoreText;
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
    private int deliveredPackages = 0;
    private int medalValue = 0;

    // Medal thresholds
    private const int BRONZE_THRESHOLD = 2;    // Value 1
    private const int SILVER_THRESHOLD = 4;    // Value 2
    private const int GOLD_THRESHOLD = 6;      // Value 3
    private const int DOUBLE_GOLD_THRESHOLD = 8; // Value 4
    private const int TRIPLE_GOLD_THRESHOLD = 10; // Value 5

    void Start()
    {
        introCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        outroCanvas.gameObject.SetActive(false);

        if (player != null)
        {
            player.SetActive(false);
        }
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

        // Reset score and start the game timer
        deliveredPackages = 0;
        medalValue = 0;
        UpdateScoreDisplay();
        currentTime = gameTime;
        isGameActive = true;
        StartCoroutine(GameTimer());
    }

    public void PackageDelivered()
    {
        if (!isGameActive) return;

        deliveredPackages++;
        UpdateScoreDisplay();
        CalculateMedalValue();
    }

    private void CalculateMedalValue()
    {
        if (deliveredPackages >= TRIPLE_GOLD_THRESHOLD)
        {
            medalValue = 5; // Triple Gold
        }
        else if (deliveredPackages >= DOUBLE_GOLD_THRESHOLD)
        {
            medalValue = 4; // Double Gold
        }
        else if (deliveredPackages >= GOLD_THRESHOLD)
        {
            medalValue = 3; // Gold
        }
        else if (deliveredPackages >= SILVER_THRESHOLD)
        {
            medalValue = 2; // Silver
        }
        else if (deliveredPackages >= BRONZE_THRESHOLD)
        {
            medalValue = 1; // Bronze
        }
        else
        {
            medalValue = 0; // No medal
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {deliveredPackages}";
        }
    }

    private void UpdateMedalDisplay()
    {
        if (medalImage == null || resultText == null) return;

        string medalText = "";
        Sprite medalSprite = null;

        switch (medalValue)
        {
            case 5:
                medalText = "¡Triple Oro!";
                medalSprite = tripleGoldMedal;
                break;
            case 4:
                medalText = "¡Doble Oro!";
                medalSprite = doubleGoldMedal;
                break;
            case 3:
                medalText = "¡Oro!";
                medalSprite = goldMedal;
                break;
            case 2:
                medalText = "¡Plata!";
                medalSprite = silverMedal;
                break;
            case 1:
                medalText = "¡Bronce!";
                medalSprite = bronzeMedal;
                break;
            default:
                medalText = "¡Sigue intentando!";
                medalSprite = null;
                break;
        }

        medalImage.sprite = medalSprite;
        medalImage.enabled = medalSprite != null;
        resultText.text = $"{medalText}\nPaquetes entregados: {deliveredPackages}";
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
        outroCanvas.gameObject.SetActive(true);

        // Update and show medal display
        UpdateMedalDisplay();

        // Disable player
        if (player != null)
        {
            player.SetActive(false);
        }

        // Here you can add code to pass the medalValue to the main game
        Debug.Log($"Game Over! Final Score: {deliveredPackages}, Medal Value: {medalValue}");
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

    // Getter for the medal value to be used by the main game
    public int GetMedalValue()
    {
        return medalValue;
    }
}