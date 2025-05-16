using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MortalBagGameOver : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        // Ocultar el panel al inicio
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Configurar el botón de continuar
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }
    }

    public void ShowGameOver(int finalScore)
    {
        // Mostrar el panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Actualizar el texto de la puntuación final
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Puntuación Final: {finalScore}";
        }
    }

    private void OnContinueClicked()
    {
        // Cargar la siguiente escena (ajusta el nombre según tu configuración)
        SceneManager.LoadScene("MainScene");
    }

    private void OnDestroy()
    {
        // Limpiar el listener del botón
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueClicked);
        }
    }
}