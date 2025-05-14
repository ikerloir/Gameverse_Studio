using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarSlider : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageOnCollision = 10f;
    [SerializeField] private float shakeIntensity = 0.5f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float fadeDuration = 1f;

    private float currentHealth;
    private MinigameManager minigameManager;
    private bool isGameOver = false;
    private Vector3 originalPosition;
    private CanvasGroup gameCanvasGroup;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        minigameManager = FindFirstObjectByType<MinigameManager>();
        if (minigameManager == null)
        {
            Debug.LogError("MinigameManager not found in the scene!");
        }

        // Get the original position for shake effect
        originalPosition = transform.position;

        // Get or add CanvasGroup to the game canvas
        gameCanvasGroup = GameObject.FindGameObjectWithTag("GameCanvas")?.GetComponent<CanvasGroup>();
        if (gameCanvasGroup == null)
        {
            Debug.LogWarning("GameCanvas not found or doesn't have CanvasGroup component!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isGameOver)
        {
            TakeDamage(damageOnCollision);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isGameOver) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over - Health reached zero!");

        // Start game over animations
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Shake effect
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            transform.position = originalPosition + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        // Fade out effect
        if (gameCanvasGroup != null)
        {
            elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                gameCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            gameCanvasGroup.alpha = 0f;
        }

        // Show outro canvas
        if (minigameManager != null)
        {
            minigameManager.GameOver();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
