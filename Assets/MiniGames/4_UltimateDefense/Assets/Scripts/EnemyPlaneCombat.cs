using UnityEngine;

public class EnemyPlaneCombat : MonoBehaviour
{
    [Header("Enemy Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Attack Settings")]
    public float baseDamage = 10f;
    public float fireCooldown = 1f;
    private float fireTimer = 0f;

    [Header("Accuracy Settings")]
    public float maxAccuracy = 0.9f;
    public float minAccuracy = 0.2f;
    public float maxEffectiveRange = 200f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public int numberOfCannons = 2;
    public Transform[] firePoints = new Transform[4];

    public Transform attackTarget;
    public GameObject floatingTextPrefab;

    // NUEVO: HealthBar
    private HealthBar healthBarInstance;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"🛩️ [EnemyPlaneCombat] Vida inicial: {currentHealth}");

        if (attackTarget == null)
        {
            GameObject obj = GameObject.FindWithTag("Player");
            if (obj != null) attackTarget = obj.transform;
        }

        // Crear barra de vida automáticamente
        GameObject hbGO = new GameObject("HealthBar");
        hbGO.transform.position = transform.position + Vector3.up * 2f;
        healthBarInstance = hbGO.AddComponent<HealthBar>();
        healthBarInstance.SetTarget(this.transform);
        healthBarInstance.SetHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        if (attackTarget == null) return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            TryAttack();
            fireTimer = fireCooldown;
        }
    }

    void TryAttack()
    {
        float distance = Vector3.Distance(transform.position, attackTarget.position);
        float accuracyFactor = Mathf.Lerp(maxAccuracy, minAccuracy, distance / maxEffectiveRange);
        accuracyFactor = Mathf.Clamp01(accuracyFactor);
        bool didHit = Random.value <= accuracyFactor;

        for (int i = 0; i < numberOfCannons; i++)
        {
            Transform fp = firePoints[i];
            if (fp == null || projectilePrefab == null) continue;

            Vector3 toTarget = (attackTarget.position - fp.position).normalized;
            Vector3 shotDirection = didHit ? toTarget : (toTarget + Random.insideUnitSphere * 0.5f).normalized;

            GameObject proj = Instantiate(projectilePrefab, fp.position, Quaternion.LookRotation(shotDirection));
            Projectile projectile = proj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetDirection(shotDirection);
                projectile.damage = baseDamage;
                projectile.targetTag = "Player";
            }

            Debug.Log($"[EnemyPlaneCombat] Disparo desde cañón {i + 1} hacia {attackTarget.name} (Hit: {didHit})");
        }
    }

    public void TakeDamage(float amount, GameObject source)
    {
        string sourceName = (source != null) ? source.name : "desconocido";
        Debug.Log($"🔥 [EnemyPlaneCombat] Impactado por {sourceName}, daño: {amount}");

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"🛩️ [EnemyPlaneCombat] Vida restante: {currentHealth}");

        ShowFloatingText(amount);

        if (healthBarInstance != null)
        {
            healthBarInstance.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void ShowFloatingText(float amount)
    {
        if (floatingTextPrefab != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            FloatingDamageText fdt = floatingText.GetComponent<FloatingDamageText>();
            if (fdt != null) fdt.SetDamage(amount);
        }
    }

    void Die()
    {
        Debug.Log("💥 [EnemyPlaneCombat] Avión destruido");

        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }

        Destroy(gameObject);
    }
}
