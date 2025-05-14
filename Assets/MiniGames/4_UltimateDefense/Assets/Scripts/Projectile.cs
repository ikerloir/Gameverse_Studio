using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 100f;
    public float damage = 10f;
    public string targetTag = "Enemy";
    public GameObject impactEffectPrefab;

    private Vector3 direction;
    private bool hasHit = false;

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
        if (hasHit) return;
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)) return;

        bool acertado = false;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage, gameObject);
            acertado = true;
        }

        hasHit = true;

        // Solo si la bala es contra 'Enemy', asumimos que es del Player y actualizamos el HUD
        if (targetTag == "Enemy")
        {
            var hud = GameObject.FindFirstObjectByType<HUDManager>();
            if (hud != null)
            {
                hud.AddShot(acertado);
            }
        }

        if (impactEffectPrefab != null)
        {
            Vector3 hitPos = other.ClosestPoint(transform.position);
            var fx = Instantiate(impactEffectPrefab, hitPos, Quaternion.LookRotation(-direction));
            Destroy(fx, 2f);
        }

        Destroy(gameObject);
    }
}
