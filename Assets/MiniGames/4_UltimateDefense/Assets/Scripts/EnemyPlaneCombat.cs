using UnityEngine;

public class EnemyPlaneCombat : MonoBehaviour, IDamageable
{
    [Header("Enemy Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Attack Settings")]
    public float baseDamage = 1f;
    public float fireCooldown = 1f;
    private float fireTimer = 0f;

    [Header("Accuracy Settings")]
    public float maxAccuracy = 0.9f;
    public float minAccuracy = 0.1f;
    public float maxEffectiveRange = 200f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public int numberOfCannons = 4;
    public Transform[] firePoints;

    [Header("Attack Target")]
    public Transform attackTarget;
    public GameObject floatingTextPrefab;

    [Header("Enemy Spawn")]
    [SerializeField] private GameObject enemyPrefab;
    public static int totalKills = 0;

    private HealthBar healthBarInstance;

    void Start()
    {
        currentHealth = maxHealth;

        if (attackTarget == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) attackTarget = player.transform;
        }

        var hbGO = new GameObject("HealthBar");
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
        float accuracy = Mathf.Lerp(maxAccuracy, minAccuracy, distance / maxEffectiveRange);
        accuracy = Mathf.Clamp01(accuracy);
        bool hit = Random.value <= accuracy;

        for (int i = 0; i < numberOfCannons && i < firePoints.Length; i++)
        {
            Transform fp = firePoints[i];
            if (fp == null || projectilePrefab == null) continue;

            Vector3 toTarget = (attackTarget.position - fp.position).normalized;
            Vector3 direction = hit ? toTarget : (toTarget + Random.insideUnitSphere * 0.5f).normalized;

            GameObject proj = Instantiate(projectilePrefab, fp.position, Quaternion.LookRotation(direction));
            if (proj.TryGetComponent(out Projectile p))
            {
                p.SetDirection(direction);
                p.damage = baseDamage;
                p.targetTag = "Player";
            }

            Debug.Log($"🟡 [EnemyPlaneCombat] Cañón {i + 1} disparó a {attackTarget.name} (Hit: {hit})");
        }
    }

    public void TakeDamage(float amount, GameObject source)
    {
        Debug.Log($"🔻 [DAMAGE] {gameObject.name} recibió {amount} daño de {source?.name}");
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        Debug.Log($"❤️ [HP] Vida restante de {gameObject.name}: {currentHealth}");

        DamageEffects.ShowFloatingText(floatingTextPrefab, transform.position, amount);

        if (healthBarInstance != null)
            healthBarInstance.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            totalKills++;
            FindObjectOfType<HUDManager>()?.AddKill();
            SpawnNewEnemy();
            Die();
        }
    }

    private void SpawnNewEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("❗ No se asignó el prefab del enemigo.");
            return;
        }

        Vector3 spawnPos = new Vector3(
            Random.Range(-10f, 10f),
            transform.position.y,
            Random.Range(15f, 25f)
        );

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        float nuevaVida = 100f + totalKills * 10f;
        float nuevoDaño = 1f + totalKills * 0.25f;

        if (newEnemy.TryGetComponent(out EnemyPlaneCombat comp))
        {
            comp.maxHealth = nuevaVida;
            comp.baseDamage = nuevoDaño;
        }

        if (newEnemy.TryGetComponent(out UnifiedDamageReceiver hp))
        {
            var field = hp.GetType().GetField("maxHealth");
            if (field != null) field.SetValue(hp, nuevaVida);
        }

        Debug.Log($"🛫 [Spawn] Enemigo creado: Vida = {nuevaVida}, Daño = {nuevoDaño}, Total kills = {totalKills}");
    }

    void Die()
    {
        Debug.Log("💥 [EnemyPlaneCombat] Avión destruido");

        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject);

        Destroy(gameObject);
    }
}