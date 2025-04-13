using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UnifiedDamageReceiver : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Effects")]
    public GameObject floatingTextPrefab;
    public Transform floatingTextSpawnPoint;
    public AudioClip hitSound;
    public AudioSource audioSource;
    public CameraShake cameraShake;

    [Header("HUD (optional)")]
    public LifeHUD lifeHUD;

    private bool isInvulnerable = true;
    private float invulnerabilityTime = 5f;

    void Start()
    {
        currentHealth = maxHealth;

        if (lifeHUD != null)
            lifeHUD.UpdateLife((int)currentHealth);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        Invoke(nameof(DisableInvulnerability), invulnerabilityTime);
    }

    void DisableInvulnerability()
    {
        isInvulnerable = false;
        Debug.Log($"🛡️ {gameObject.name} ya no es invulnerable.");
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (isInvulnerable)
        {
            Debug.Log($"🛡️ {gameObject.name} ignoró daño por invulnerabilidad inicial.");
            return;
        }

        Debug.Log($"🔺 [DAMAGE] {gameObject.name} recibió {amount} daño de {source?.name}");
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        Debug.Log($"❤️ [HP] Vida restante de {gameObject.name}: {currentHealth}");

        Vector3 spawnPos = (floatingTextSpawnPoint != null) ? floatingTextSpawnPoint.position : transform.position;
        DamageEffects.ShowFloatingText(floatingTextPrefab, spawnPos, amount);
        DamageEffects.ShakeCamera(cameraShake);
        DamageEffects.PlaySound(audioSource, hitSound);

        if (lifeHUD != null)
            lifeHUD.UpdateLife((int)currentHealth);

        var hud = FindObjectOfType<HUDManager>();
        hud?.UpdateHealth((int)currentHealth);
        hud?.ShowDamageReceived(amount);

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        Debug.Log($"💀 [UnifiedDamageReceiver] {gameObject.name} ha sido destruido.");
        Destroy(gameObject);
    }
}
