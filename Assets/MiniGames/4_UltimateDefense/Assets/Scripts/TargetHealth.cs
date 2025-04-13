using UnityEngine;

public class TargetHealth : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float maxHealth = 1000f;
    private float currentHealth;

    [Header("Tags de efecto")]
    public string bonusDamageTag = "HeavyAttack";
    public string healOnHitTag = "HealAttack";

    [Header("Visuals")]
    public GameObject floatingTextPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, GameObject source)
    {
        float finalDamage = amount;

        if (source != null && source.CompareTag(bonusDamageTag))
            finalDamage *= 1.5f;

        currentHealth = Mathf.Clamp(currentHealth - finalDamage, 0f, maxHealth);

        DamageEffects.ShowFloatingText(floatingTextPrefab, transform.position, finalDamage);
        DamageEffects.TriggerPlayerFlash(this.gameObject);

        if (currentHealth <= 0)
            Die();
    }

    public void HealOnAttack(GameObject source, float amount)
    {
        if (source != null && source.CompareTag(healOnHitTag))
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }
    }

    void Die()
    {
        Debug.Log("[TargetHealth] ¡Destruido!");
        Destroy(gameObject);
    }
}