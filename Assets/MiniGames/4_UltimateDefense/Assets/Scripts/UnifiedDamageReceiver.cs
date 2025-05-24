using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    public AudioClip explosionSound;
    public float explosionSoundVolume = 1f;
    public AudioSource audioSource;
    public CameraShake cameraShake;

    [Header("Pooling Tags")]
    public string explosionEffectTag = "Explosion";

    [Header("HUD (optional)")]
    public LifeHUD lifeHUD;

    private bool isInvulnerable = true;
    private float invulnerabilityTime = 5f;

    private Dictionary<GameObject, float> recentSources = new Dictionary<GameObject, float>();
    private float sourceCooldown = 0.1f;

    private static HUDManager hud;

    void Start()
    {
        currentHealth = maxHealth;

        if (lifeHUD != null)
            lifeHUD.UpdateLife((int)currentHealth);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (hud == null)
            hud = FindObjectOfType<HUDManager>();

        Invoke(nameof(DisableInvulnerability), invulnerabilityTime);
    }

    void DisableInvulnerability()
    {
        isInvulnerable = false;
        Debug.Log($"🛡️ [{gameObject.name}] Invulnerabilidad desactivada.");
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (isInvulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

        Vector3 spawnPos = floatingTextSpawnPoint != null ? floatingTextSpawnPoint.position : transform.position;

        DamageEffects.ShowFloatingText(floatingTextPrefab, spawnPos, amount);
        DamageEffects.ShakeCamera(cameraShake);

        if (!recentSources.ContainsKey(source) || Time.time - recentSources[source] > sourceCooldown)
        {
            DamageEffects.PlaySound(audioSource, hitSound);
            recentSources[source] = Time.time;
        }

        if (lifeHUD != null)
            lifeHUD.UpdateLife((int)currentHealth);

        if (hud != null)
        {
            hud.UpdatePlayerHealth(currentHealth, maxHealth);
            hud.ShowDamageReceived(amount);
        }

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        Debug.Log($"💀 [{gameObject.name}] destruido en {transform.position}");

        // Pool explosion
        if (!string.IsNullOrEmpty(explosionEffectTag))
        {
            GameObject fx = ObjectPooler.Instance.SpawnFromPool(explosionEffectTag, transform.position, Quaternion.identity);
            StartCoroutine(DisableAfterSeconds(fx, 7f));
        }

        // Pool sonido
        if (explosionSound != null)
        {
            GameObject audioObj = ObjectPooler.Instance.SpawnFromPool("ExplosionAudio", transform.position, Quaternion.identity);
            AudioSource pooledSource = audioObj.GetComponent<AudioSource>();
            if (pooledSource == null)
                pooledSource = audioObj.AddComponent<AudioSource>();

            pooledSource.clip = explosionSound;
            pooledSource.volume = explosionSoundVolume;
            pooledSource.spatialBlend = 0f;
            pooledSource.Play();

            StartCoroutine(DisableAfterSeconds(audioObj, explosionSound.length));
        }

        DamageEffects.ShakeCamera(cameraShake);
        gameObject.SetActive(false);
    }

    IEnumerator DisableAfterSeconds(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
