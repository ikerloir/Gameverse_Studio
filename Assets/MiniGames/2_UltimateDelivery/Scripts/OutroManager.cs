using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OutroManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject outroCanvas;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Image medalImage;
    [SerializeField] private Sprite bronzeMedalSprite;
    [SerializeField] private Sprite silverMedalSprite;
    [SerializeField] private Sprite goldMedalSprite;
    [SerializeField] private Sprite doubleGoldMedalSprite;
    [SerializeField] private Sprite tripleGoldMedalSprite;

    [Header("Manager References")]
    [SerializeField] private DeliveryScoreManager scoreManager;

    private void Start()
    {
        if (scoreManager == null)
        {
            Debug.LogError("DeliveryScoreManager reference is missing! Please assign it in the inspector.");
        }

        if (outroCanvas != null)
        {
            outroCanvas.SetActive(false);
        }
        HideMedal();
        Debug.Log("OutroManager started");
    }

    public void ShowOutro()
    {
        if (outroCanvas != null)
        {
            outroCanvas.SetActive(true);
            UpdateOutroDisplay();
            Debug.Log("Outro display shown");
        }
        else
        {
            Debug.LogError("Outro canvas is null!");
        }
    }

    private void UpdateOutroDisplay()
    {
        if (scoreManager != null)
        {
            int currentScore = scoreManager.GetScore();
            Debug.Log($"Updating outro display with score: {currentScore}");

            // Update final score and medal text
            if (finalScoreText != null)
            {
                string medalText = GetMedalText(currentScore);
                finalScoreText.text = $"{medalText}\nPaquetes entregados: {currentScore}";
                Debug.Log($"Updated score text to: {finalScoreText.text}");
            }
            else
            {
                Debug.LogError("Final score text is null!");
            }

            // Show the appropriate medal
            ShowMedal(currentScore);
        }
        else
        {
            Debug.LogError("Score manager is null!");
        }
    }

    private string GetMedalText(int score)
    {
        if (score >= 9)
            return "¡Triple Oro!";
        else if (score >= 7)
            return "¡Doble Oro!";
        else if (score >= 5)
            return "¡Oro!";
        else if (score >= 3)
            return "¡Plata!";
        else if (score >= 0)
            return "¡Bronce!";
        else
            return "¡Sigue intentando!";
    }

    private void ShowMedal(int score)
    {
        if (medalImage != null)
        {
            medalImage.gameObject.SetActive(true);
            Sprite selectedSprite = null;

            if (score >= 9)
                selectedSprite = tripleGoldMedalSprite;
            else if (score >= 7)
                selectedSprite = doubleGoldMedalSprite;
            else if (score >= 5)
                selectedSprite = goldMedalSprite;
            else if (score >= 3)
                selectedSprite = silverMedalSprite;
            else if (score >= 0)
                selectedSprite = bronzeMedalSprite;

            if (selectedSprite != null)
            {
                medalImage.sprite = selectedSprite;
                Debug.Log($"Showing medal for score: {score}");
            }
            else
            {
                Debug.LogWarning($"No sprite found for score: {score}");
                HideMedal();
            }
        }
        else
        {
            Debug.LogError("Medal image is null!");
        }
    }

    private void HideMedal()
    {
        if (medalImage != null)
        {
            medalImage.gameObject.SetActive(false);
            Debug.Log("Medal hidden");
        }
    }
}