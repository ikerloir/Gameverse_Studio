using UnityEngine;
using System.Collections.Generic;

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

    [Header("HUD (optional)")]
    public LifeHUD lifeHUD;

    private bool isInvulnerable = true;
    private float invulnerabilityTime = 5f;

    private Dictionary<GameObject, float> recentSources = new Dictionary<GameObject, float>();
    private float sourceCooldown = 0.1f;

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
        Debug.Log($"🛡️ [{gameObject.name}] Invulnerabilidad desactivada.");
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (isInvulnerable)
        {
            Debug.Log($"🛡️ [{gameObject.name}] Ignoró daño por invulnerabilidad inicial.");
            return;
        }

        Debug.Log($"🔺 [DAMAGE] {gameObject.name} recibió {amount} daño de {source?.name}");
        Debug.Log($"📍 Posición actual del objeto: {transform.position}");
        if (source != null)
            Debug.Log($"📍 Posición del proyectil (source): {source.transform.position}");

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        Debug.Log($"❤️ [HP] {gameObject.name} vida restante: {currentHealth}");

        Vector3 spawnPos = (floatingTextSpawnPoint != null) ? floatingTextSpawnPoint.position : transform.position;
        Debug.Log($"📝 Spawn del texto flotante en: {spawnPos}");

        DamageEffects.ShowFloatingText(floatingTextPrefab, spawnPos, amount);
        DamageEffects.ShakeCamera(cameraShake);

        if (!recentSources.ContainsKey(source) || Time.time - recentSources[source] > sourceCooldown)
        {
            DamageEffects.PlaySound(audioSource, hitSound);
            recentSources[source] = Time.time;
            Debug.Log($"🔊 Sonido de impacto reproducido.");
        }

        if (lifeHUD != null)
        {
            lifeHUD.UpdateLife((int)currentHealth);
            Debug.Log($"🖥️ HUD actualizado con vida: {currentHealth}");
        }

        HUDManager hud = FindFirstObjectByType<HUDManager>();
        if (hud != null)
        {
            hud.UpdatePlayerHealth(currentHealth, maxHealth);
            hud.ShowDamageReceived(amount);
            Debug.Log($"🖥️ HUDManager notificado del daño recibido.");
        }

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        Debug.Log($"💀 [UnifiedDamageReceiver] {gameObject.name} ha sido destruido en {transform.position}.");

        GameObject explosion = new GameObject("AirplaneExplosion");
        explosion.transform.position = transform.position;
        if (explosionSound != null)
        {
            GameObject audioObj = new GameObject("ExplosionSound");
            audioObj.transform.position = transform.position;
            var source = audioObj.AddComponent<AudioSource>();
            source.clip = explosionSound;
            source.volume = explosionSoundVolume;
            source.spatialBlend = 0f;
            source.Play();
            Destroy(audioObj, explosionSound.length);

            Debug.Log($"💥 Sonido de explosión reproducido en {transform.position}");
        }

        // Configuración de Partículas
        ParticleSystem firePS = explosion.AddComponent<ParticleSystem>();
        firePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = firePS.main;
        main.duration = 2.0f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(1.0f, 2.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(10f, 30f);
        main.startSize = new ParticleSystem.MinMaxCurve(3f, 6f);
        main.startColor = new ParticleSystem.MinMaxGradient(Color.yellow, Color.red);
        main.gravityModifier = 0.3f;
        main.loop = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 500;

        var emission = firePS.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, 250, 350) });

        var shape = firePS.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 2.0f;

        var colorOverLifetime = firePS.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(Color.yellow, 0.0f),
                new GradientColorKey(Color.red, 0.5f),
                new GradientColorKey(new Color(0.05f, 0.05f, 0.05f), 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.5f, 0.5f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = grad;

        var sizeOverLifetime = firePS.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 1.0f);
        curve.AddKey(0.5f, 1.5f);
        curve.AddKey(1.0f, 0.0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, curve);

        var noise = firePS.noise;
        noise.enabled = true;
        noise.strength = 2.0f;
        noise.frequency = 0.5f;
        noise.scrollSpeed = 0.2f;

        var renderer = firePS.GetComponent<ParticleSystemRenderer>();
        Shader shader = Shader.Find("Particles/Standard Unlit");
        Texture2D whiteTex = new Texture2D(1, 1);
        whiteTex.SetPixel(0, 0, Color.white);
        whiteTex.Apply();
        if (shader != null)
        {
            Material mat = new Material(shader);
            mat.SetTexture("_MainTex", whiteTex);
            mat.SetColor("_Color", Color.white);
            mat.SetFloat("_Mode", 2);
            renderer.material = mat;
        }

        firePS.Play();
        Debug.Log($"🔥 Partículas de explosión creadas y activadas.");

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        }

        DamageEffects.ShakeCamera(cameraShake);

        Destroy(explosion, 7f);
        Destroy(gameObject);
    }
}
