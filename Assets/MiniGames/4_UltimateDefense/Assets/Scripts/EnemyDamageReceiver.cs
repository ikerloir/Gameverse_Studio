using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    [Header("Vida")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Efectos")]
    public GameObject floatingTextPrefab;
    public Transform floatingTextSpawnPoint;
    public AudioClip hitSound;
    public AudioSource audioSource;
    public CameraShake cameraShake;

    void Start()
    {
        currentHealth = maxHealth;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LP")) // El proyectil del jugador
        {
            float damage = 10f;

            ApplyDamage(damage, collision.contacts[0].point);

            Destroy(collision.gameObject);
        }
    }

    void ApplyDamage(float amount, Vector3 hitPoint)
    {
        currentHealth -= amount;

        // Mostrar texto flotante
        if (floatingTextPrefab != null)
        {
            Vector3 spawnPos = floatingTextSpawnPoint != null ? floatingTextSpawnPoint.position : hitPoint;

            GameObject textObj = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);
            FloatingDamageText text = textObj.GetComponent<FloatingDamageText>();

            if (text != null)
                text.SetDamage(amount);
        }

        // Reproducir sonido
        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        // Vibrar cámara
        if (cameraShake != null)
            cameraShake.TriggerShake();

        // Verificar si muere
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("💀 El enemigo ha sido destruido.");
        Destroy(gameObject);
    }
}
