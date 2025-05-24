using UnityEngine;
using System.Collections;

public class EnemyPlaneCombat : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    public float baseDamage = 10f;
    private float currentHealth;
    private bool isDead = false;

    public GameObject floatingTextPrefab;
    public AudioClip explosionSound;
    public float explosionSoundVolume = 1f;

    public GameObject enemyProjectilePrefab;
    public Transform[] firePoints;
    public float fireRate = 2f;
    private float fireCooldown = 0f;

    private Transform playerTarget;
    private static HUDManager hud;

    [Header("Pooling Tags")]
    public string explosionTag = "Explosion";
    public string impactEffectTag = "Impact";

    [Header("Impact Settings")]
    public float effectOffset = 0.1f;
    [Range(0f, 1f)]
    public float hitProbability = 0.8f;

    void Start()
    {
        currentHealth = maxHealth;

        GameObject player = GameObject.Find("LP");
        if (player != null)
        {
            playerTarget = player.transform;
        }

        if (hud == null)
        {
            hud = FindObjectOfType<HUDManager>();
        }
    }

    void Update()
    {
        if (isDead || playerTarget == null) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            FireAtPlayer();
            fireCooldown = 1f / fireRate;
        }
    }

    void FireAtPlayer()
    {
        foreach (Transform firePoint in firePoints)
        {
            if (firePoint == null || enemyProjectilePrefab == null) continue;

            GameObject projectile = Instantiate(enemyProjectilePrefab, firePoint.position, firePoint.rotation);
            if (projectile.TryGetComponent(out Projectile projScript))
            {
                Vector3 direction = (playerTarget.position - firePoint.position).normalized;
                projScript.SetDirection(direction);
            }
        }
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (isDead) return;

        Vector3 hitPoint = source != null ? source.transform.position : transform.position;
        Vector3 spawnPos = hitPoint + (transform.position - hitPoint).normalized * effectOffset;

        if (!string.IsNullOrEmpty(impactEffectTag))
        {
            GameObject fx = ObjectPooler.Instance.SpawnFromPool(impactEffectTag, spawnPos, Quaternion.identity);
            StartCoroutine(DisableAfterSeconds(fx, 2f));
        }

        if (Random.value > hitProbability) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        DamageEffects.ShowFloatingText(floatingTextPrefab, transform.position, amount);

        hud?.AddDamageDealt(amount);

        if (currentHealth <= 0f)
        {
            isDead = true;
            hud?.AddKill(maxHealth);
            Die();
        }
    }

    void Die()
    {
        if (!string.IsNullOrEmpty(explosionTag))
        {
            GameObject fx = ObjectPooler.Instance.SpawnFromPool(explosionTag, transform.position, Quaternion.identity);
            StartCoroutine(DisableAfterSeconds(fx, 7f));
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        }

        gameObject.SetActive(false);
    }

    IEnumerator DisableAfterSeconds(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
