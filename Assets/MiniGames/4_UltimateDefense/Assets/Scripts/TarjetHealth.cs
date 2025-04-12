using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public string bonusDamageTag = "HeavyAttack";
    public string healOnHitTag = "HealAttack";
    public GameObject floatingTextPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"[TargetHealth] Salud inicial: {currentHealth}");
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (source == null) source = gameObject;

        float finalDamage = amount;

        if (source.CompareTag(bonusDamageTag))
        {
            finalDamage *= 1.5f;
            Debug.Log("[TargetHealth] Daño potenciado aplicado.");
        }

        currentHealth -= finalDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"[TargetHealth] Daño recibido: {finalDamage}. Vida restante: {currentHealth}");

        ShowFloatingText(finalDamage);
        TriggerDamageFlash();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void HealOnAttack(GameObject source, float amount)
    {
        if (source != null && source.CompareTag(healOnHitTag))
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            Debug.Log($"[TargetHealth] Curación recibida: {amount}. Vida actual: {currentHealth}");
        }
    }

    void ShowFloatingText(float amount)
    {
        if (floatingTextPrefab != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            FloatingDamageText fdt = floatingText.GetComponent<FloatingDamageText>();
            if (fdt != null)
            {
                fdt.SetDamage(amount);
            }

            Debug.Log("[TargetHealth] Texto flotante instanciado.");
        }
    }

    void TriggerDamageFlash()
    {
        if (CompareTag("Player"))
        {
            ScreenDamageFlash flash = FindFirstObjectByType<ScreenDamageFlash>();
            if (flash != null)
            {
                flash.TriggerFlash();
                Debug.Log("[TargetHealth] Flash de daño activado.");
            }
        }
    }

    void Die()
    {
        Debug.Log("[TargetHealth] ¡Objeto destruido!");
        Destroy(gameObject);
    }
}
