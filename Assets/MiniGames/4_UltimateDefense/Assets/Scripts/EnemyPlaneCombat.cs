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

    public GameObject enemyProjectilePrefab; // Prefab del proyectil a disparar
    public Transform[] firePoints; // Los 4 puntos de disparo
    public float fireRate = 2f;
    private float fireCooldown = 0f;

    private Transform playerTarget;

    private void Start()
    {
        currentHealth = maxHealth;

        // Buscar al jugador con nombre "LP"
        GameObject player = GameObject.Find("LP");
        if (player != null)
        {
            playerTarget = player.transform;
        }
    }

    private void Update()
    {
        if (!isDead && playerTarget != null)
        {
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                FireAtPlayer();
                fireCooldown = 1f / fireRate;
            }
        }
    }

    private void FireAtPlayer()
    {
        foreach (Transform firePoint in firePoints)
        {
            if (firePoint != null && enemyProjectilePrefab != null)
            {
                GameObject projectile = Instantiate(enemyProjectilePrefab, firePoint.position, firePoint.rotation);
                Projectile projScript = projectile.GetComponent<Projectile>();

                if (projScript != null)
                {
                    Vector3 direction = (playerTarget.position - firePoint.position).normalized;
                    projScript.SetDirection(direction);
                }
            }
        }
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
        // Crear GameObject de explosión
        GameObject explosion = new GameObject("AirplaneExplosion");
        explosion.transform.position = transform.position;

        // Crear el sistema de partículas principal (fuego)
        ParticleSystem firePS = explosion.AddComponent<ParticleSystem>();
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
        emission.burstCount = 1;
        emission.SetBurst(0, new ParticleSystem.Burst(0.0f, 250, 350));

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

        // Asignar material seguro con textura blanca y shader additive
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
            mat.SetFloat("_Mode", 2); // Additive
            renderer.material = mat;
        }

        firePS.Play();

        // Crear sistema de humo
        GameObject smokeObj = new GameObject("Smoke");
        smokeObj.transform.parent = explosion.transform;
        smokeObj.transform.localPosition = Vector3.zero;
        var smokePS = smokeObj.AddComponent<ParticleSystem>();
        var smokeMain = smokePS.main;
        smokeMain.startLifetime = 5f;
        smokeMain.startSpeed = 1f;
        smokeMain.startSize = 5f;
        smokeMain.startColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        smokeMain.gravityModifier = -0.1f;
        smokeMain.loop = false;
        smokeMain.simulationSpace = ParticleSystemSimulationSpace.World;
        var smokeEmission = smokePS.emission;
        smokeEmission.rateOverTime = 0;
        smokeEmission.SetBurst(0, new ParticleSystem.Burst(0f, 100));
        var smokeShape = smokePS.shape;
        smokeShape.shapeType = ParticleSystemShapeType.Sphere;
        smokeShape.radius = 2.5f;
        var smokeRenderer = smokePS.GetComponent<ParticleSystemRenderer>();
        smokeRenderer.material = renderer.material; // Reusar el material blanco con additive (opcionalmente puedes usar un shader Alpha Blended)
        smokePS.Play();

        // Crear sistema de chispas
        GameObject sparksObj = new GameObject("Sparks");
        sparksObj.transform.parent = explosion.transform;
        sparksObj.transform.localPosition = Vector3.zero;
        var sparksPS = sparksObj.AddComponent<ParticleSystem>();
        var sparksMain = sparksPS.main;
        sparksMain.startLifetime = 0.5f;
        sparksMain.startSpeed = 50f;
        sparksMain.startSize = 0.3f;
        sparksMain.startColor = Color.yellow;
        sparksMain.gravityModifier = 0.1f;
        sparksMain.loop = false;
        sparksMain.simulationSpace = ParticleSystemSimulationSpace.World;
        var sparksEmission = sparksPS.emission;
        sparksEmission.rateOverTime = 0;
        sparksEmission.SetBurst(0, new ParticleSystem.Burst(0f, 150));
        var sparksShape = sparksPS.shape;
        sparksShape.shapeType = ParticleSystemShapeType.Cone;
        sparksShape.angle = 45f;
        sparksShape.radius = 0.5f;
        var sparksRenderer = sparksPS.GetComponent<ParticleSystemRenderer>();
        sparksRenderer.material = renderer.material;
        sparksPS.Play();

        // Crear luz puntual temporal
        var lightObj = new GameObject("ExplosionLight");
        lightObj.transform.parent = explosion.transform;
        lightObj.transform.localPosition = Vector3.zero;
        var light = lightObj.AddComponent<Light>();
        light.type = LightType.Point;
        light.intensity = 10f;
        light.range = 20f;
        light.color = Color.yellow;
        Destroy(lightObj, 0.5f);

        // Sonido de explosión
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        }

        // Destruir todo el sistema después de 7 segundos
        Destroy(explosion, 7f);

        // Destruir el avión
        Destroy(gameObject);
    }


}