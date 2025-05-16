using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MortalBagOutro : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image medalImage;
    [SerializeField] private Sprite bronzeMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite doubleGoldMedal;
    [SerializeField] private Sprite tripleGoldMedal;

    private void Awake()
    {
        // Asegurarse de que existe un EventSystem
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Añadir el componente NextGameButton al botón de continuar
        if (continueButton != null)
        {
            continueButton.gameObject.AddComponent<NextGameButton>();
        }
    }

    private void Start()
    {
        // Configurar el botón de menú principal
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        // Mostrar la medalla y mensaje basado en la puntuación
        if (MortalBagScoreManager.Instance != null)
        {
            int scaledScore = MortalBagScoreManager.Instance.GetScore() * 3; // Usar la escala existente
            scaledScore = Mathf.Min(scaledScore, 100); // Limitar a 100

            // Mostrar medalla y mensaje basado en la puntuación escalada
            if (medalImage != null)
            {
                if (scaledScore >= 81)
                {
                    medalImage.sprite = tripleGoldMedal;
                    messageText.text = "¡Triple Oro! ¡Increíble trabajo!";
                }
                else if (scaledScore >= 61)
                {
                    medalImage.sprite = doubleGoldMedal;
                    messageText.text = "¡Doble Oro! ¡Excelente trabajo!";
                }
                else if (scaledScore >= 41)
                {
                    medalImage.sprite = goldMedal;
                    messageText.text = "¡Medalla de Oro! ¡Gran trabajo!";
                }
                else if (scaledScore >= 21)
                {
                    medalImage.sprite = silverMedal;
                    messageText.text = "¡Medalla de Plata! ¡Buen trabajo!";
                }
                else
                {
                    medalImage.sprite = bronzeMedal;
                    messageText.text = "¡Medalla de Bronce! ¡Sigue mejorando!";
                }
            }
        }
    }

    private void OnMainMenuClicked()
    {
        // Cargar el menú principal del juego
        SceneManager.LoadScene("Menu");
    }

    private void OnDestroy()
    {
        // Limpiar los listeners de los botones
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }
    }
}