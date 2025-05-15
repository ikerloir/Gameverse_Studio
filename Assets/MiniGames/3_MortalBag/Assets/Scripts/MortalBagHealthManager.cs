using UnityEngine;
using UnityEngine.UI;

public class MortalBagHealthManager : MonoBehaviour
{
    public static MortalBagHealthManager Instance;
    public int maxLives = 3;
    public Image[] lifeImages; // Array de imágenes de vidas

    private int currentLives;

    void Awake()
    {
        Debug.Log("MortalBagHealthManager Awake");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MortalBagHealthManager inicializado como singleton");
        }
        else
        {
            Debug.Log("Ya existe una instancia de MortalBagHealthManager, destruyendo duplicado");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("MortalBagHealthManager Start");
        currentLives = maxLives;
        Debug.Log($"Vidas iniciales: {currentLives}");
        UpdateLivesDisplay();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Tomando daño: {damage}. Vidas actuales: {currentLives}");
        currentLives -= damage;
        if (currentLives < 0) currentLives = 0;

        Debug.Log($"Vidas después del daño: {currentLives}");
        UpdateLivesDisplay();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesDisplay()
    {
        Debug.Log("Actualizando display de vidas");

        // Actualizar imágenes
        if (lifeImages != null && lifeImages.Length > 0)
        {
            Debug.Log($"Actualizando {lifeImages.Length} imágenes de vidas");
            for (int i = 0; i < lifeImages.Length; i++)
            {
                if (lifeImages[i] != null)
                {
                    bool shouldBeVisible = i < currentLives;
                    lifeImages[i].enabled = shouldBeVisible;
                    Debug.Log($"Imagen {i}: {(shouldBeVisible ? "visible" : "oculta")}");
                }
                else
                {
                    Debug.LogError($"lifeImages[{i}] es null!");
                }
            }
        }
        else
        {
            Debug.LogError("lifeImages es null o está vacío!");
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        // Aquí puedes añadir la lógica para el game over
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}