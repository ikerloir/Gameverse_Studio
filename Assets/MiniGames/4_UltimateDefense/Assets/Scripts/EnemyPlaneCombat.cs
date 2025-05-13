using UnityEngine;

public class EnemyPlaneCombat : MonoBehaviour, IDamageable
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
    public float minAccuracy = 0.1f;
    public float maxEffectiveRange = 200f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform[] firePoints;

    [Header("Attack Target")]
    public Transform attackTarget;

    [Header("Effects")]
    public GameObject floatingTextPrefab;
    public GameObject explosionPrefab;
    public AudioClip explosionSound;
    public float explosionSoundVolume = 1f;

    private void Start()
    {
        currentHealth = maxHealth;

        if (attackTarget == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) attackTarget = player.transform;
        }
    }

    private void Update()
    {
        if (attackTarget == null) return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            TryAttack();
            fireTimer = fireCooldown;
        }
    }

    private void TryAttack()
    {
        float distance = Vector3.Distance(transform.position, attackTarget.position);
        float accuracy = Mathf.Lerp(maxAccuracy, minAccuracy, distance / maxEffectiveRange);
        accuracy = Mathf.Clamp01(accuracy);
        bool hit = Random.value <= accuracy;

        foreach (Transform fp in firePoints)
        {
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

            Debug.Log($"🟡 [EnemyPlaneCombat] Cañón disparó a {attackTarget.name} (Hit: {hit})");
        }
    }

    public void TakeDamage(float amount, GameObject source)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

        DamageEffects.ShowFloatingText(floatingTextPrefab, transform.position, amount);

        if (currentHealth <= 0f)
        {
            var hud = FindFirstObjectByType<HUDManager>();
            if (hud != null)
            {
                hud.AddKill(maxHealth);
            }
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("💥 [EnemyPlaneCombat] Avión destruido");

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
