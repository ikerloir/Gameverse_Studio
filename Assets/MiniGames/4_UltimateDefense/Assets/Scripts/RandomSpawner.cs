using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RandomSpawner : MonoBehaviour, IDamageable
{
    [Header("Área de spawn (usar un GameObject con BoxCollider)")]
    public Collider spawnAreaCollider;
    public GameObject floatingHPTextPrefab;
    public float extraLife = 50f;
    public float maxHealth = 100f;
    public float rotationSpeed = 50f;
    public AudioClip explosionSound;
    public float explosionSoundVolume = 1f;

    private float currentHealth;
    private float spawnInterval = 20f;
    private float stayTime = 10f;
    private float blinkTime = 4f;
    private Renderer[] renderers;
    private Collider objectCollider;
    private bool isActive = false;
    private float timer;
    private float blinkTimer;
    private bool visible = true;

    private HUDManager hudManager;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        objectCollider = GetComponent<Collider>();
 

        if (spawnAreaCollider == null)
            Debug.LogError("⚠️ No se ha asignado un Spawn Area Collider al RandomSpawner.");

        SetActive(false);
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (!isActive && timer <= 0)
        {
            Respawn();
        }

        if (isActive)
        {
            stayTime -= Time.deltaTime;
            RotateObject();

            if (stayTime <= blinkTime)
            {
                BlinkEffect();
            }

            if (stayTime <= 0)
            {
                SetActive(false);
                timer = spawnInterval;
            }
        }
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void BlinkEffect()
    {
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= 0.25f)
        {
            visible = !visible;
            SetRenderers(visible);
            blinkTimer = 0f;
        }
    }

    private void SetRenderers(bool state)
    {
        foreach (var rend in renderers)
        {
            rend.enabled = state;
        }
    }

    private void Respawn()
    {
        if (spawnAreaCollider != null)
        {
            Bounds bounds = spawnAreaCollider.bounds;
            Vector3 randomPos = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );

            transform.position = randomPos;
        }

        stayTime = 10f;
        currentHealth = maxHealth;
        isActive = true;
        SetRenderers(true);
        if (objectCollider != null) objectCollider.enabled = true;
    }

    private void SetActive(bool state)
    {
        isActive = state;
        SetRenderers(state);
        if (objectCollider != null) objectCollider.enabled = state;
        visible = state;
    }

    public void TakeDamage(float amount, GameObject source)
    {
        if (!isActive) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GivePlayerLife();
        ShowFloatingHP();
        ShowExplosion();
        SetActive(false);
        timer = spawnInterval;
    }

    private void GivePlayerLife()
    {
        if (hudManager != null)
        {
            hudManager.AddLife(extraLife);
        }
    }

    private void ShowFloatingHP()
    {
        if (floatingHPTextPrefab != null)
        {
            GameObject textObj = Instantiate(floatingHPTextPrefab, transform.position + Vector3.up, Quaternion.identity);
            TextMesh text = textObj.GetComponent<TextMesh>();
            if (text != null)
            {
                text.text = "+" + extraLife + " HP";
                text.color = Color.green;
            }
            Destroy(textObj, 2f);
        }
    }

    private void ShowExplosion()
    {
        GameObject explosion = new GameObject("GreenBonusExplosion");
        explosion.transform.position = transform.position;

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, explosionSoundVolume);
        }

        ParticleSystem ps = explosion.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.duration = 1.5f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 1.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(5f, 15f);
        main.startSize = new ParticleSystem.MinMaxCurve(1f, 3f);
        main.startColor = new ParticleSystem.MinMaxGradient(Color.green, Color.white);
        main.gravityModifier = 0.1f;
        main.loop = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 200;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 100, 150) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 1.5f;

        ps.Play();
        Destroy(explosion, 3f);
    }
}
