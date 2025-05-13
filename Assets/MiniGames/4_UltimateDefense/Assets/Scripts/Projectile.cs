using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 100f;
    public float damage = 10f;
    public string targetTag = "Enemy";
    public GameObject impactEffectPrefab;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag))
            return;

        bool acertado = false;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage, gameObject);
            acertado = true;
        }

        var hud = FindFirstObjectByType<HUDManager>();
        if (hud != null)
        {
            hud.AddShot(acertado);
        }

        if (impactEffectPrefab != null)
        {
            var fx = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(gameObject);
    }
}
