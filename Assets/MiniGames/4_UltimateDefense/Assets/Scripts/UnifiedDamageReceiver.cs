using UnityEngine;

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
        Debug.Log($"🛡️ {gameObject.name} ya no es invulnerable.");
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (isInvulnerable)
        {
            Debug.Log($"🛡️ {gameObject.name} ignoró daño por invulnerabilidad inicial.");
            return;
        }

        Debug.Log($"🔺 [DAMAGE] {gameObject.name} recibió {amount} daño de {source?.name}");
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        Debug.Log($"❤️ [HP] Vida restante de {gameObject.name}: {currentHealth}");

        Vector3 spawnPos = (floatingTextSpawnPoint != null) ? floatingTextSpawnPoint.position : transform.position;
        DamageEffects.ShowFloatingText(floatingTextPrefab, spawnPos, amount);
        DamageEffects.ShakeCamera(cameraShake);
        DamageEffects.PlaySound(audioSource, hitSound);

        if (lifeHUD != null)
            lifeHUD.UpdateLife((int)currentHealth);

        HUDManager hud = FindFirstObjectByType<HUDManager>();
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
        Debug.Log($"💀 [UnifiedDamageReceiver] {gameObject.name} ha sido destruido.");

        GameObject explosion = new GameObject("AirplaneExplosion");
        explosion.transform.position = transform.position;
        // Sonido 2D garantizado
        if (explosionSound != null)
        {
            GameObject audioObj = new GameObject("ExplosionSound");
            audioObj.transform.position = transform.position;
            var source = audioObj.AddComponent<AudioSource>();
            source.clip = explosionSound;
            source.volume = explosionSoundVolume;
            source.spatialBlend = 0f; // Forzar 2D
            source.Play();
            Destroy(audioObj, explosionSound.length);
        }

        // Fuego
        ParticleSystem firePS = explosion.AddComponent<ParticleSystem>();
        firePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // DETENER antes de configurar
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

        // (Resto de humo, chispas y luz repetir misma lógica de Stop() antes de configurar y usar SetBursts correctamente)

        // Sonido
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        }

        // Shake cámara
        DamageEffects.ShakeCamera(cameraShake);

        Destroy(explosion, 7f);
        Destroy(gameObject);
    }

}