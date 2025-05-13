using UnityEngine;

public class EnemyPlaneCombat : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public float baseDamage = 10f;
    private float currentHealth;
    private bool isDead = false;

    public GameObject floatingTextPrefab;
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public float explosionSoundVolume = 1f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

        DamageEffects.ShowFloatingText(floatingTextPrefab, transform.position, amount);

        var hud = GameObject.FindFirstObjectByType<HUDManager>();
        if (hud != null)
        {
            hud.AddDamageDealt(amount);
        }

        if (currentHealth <= 0f)
        {
            isDead = true;

            if (hud != null)
            {
                hud.AddKill(maxHealth);
            }

            Die();
        }
    }

    private void Die()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 5f);
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        }

        Destroy(gameObject);
    }
}
