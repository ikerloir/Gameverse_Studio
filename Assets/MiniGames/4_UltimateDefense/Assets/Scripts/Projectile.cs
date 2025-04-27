using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 100f;
    public float damage = 10f;
    public float lifeTime = 5f;
    public string targetTag = "Enemy";

    public GameObject impactEffectPrefab;
    public AudioClip impactSound;
    public float impactSoundVolume = 1f;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (TryGetComponent(out Rigidbody rb) == false)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Projectile] Trigger con: {other.name} | Tag: {other.tag}");

        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag))
            return;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage, this.gameObject);
        }

        if (impactEffectPrefab != null)
        {
            var fx = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position, impactSoundVolume);
        }

        Destroy(gameObject);
    }
}