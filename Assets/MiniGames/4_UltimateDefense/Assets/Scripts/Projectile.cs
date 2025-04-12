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
        Debug.Log($"🚀 [Projectile] Bala creada: {gameObject.name}");
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Projectile] Trigger con: {other.name} | Tag: {other.tag}");

        // Quita esta validación mientras depuras, luego puedes volver a activarla
        // if (!other.CompareTag(targetTag)) return;

        // Buscar componentes relevantes
        var th = other.GetComponent<TargetHealth>() ?? other.GetComponentInParent<TargetHealth>();
        var epc = other.GetComponent<EnemyPlaneCombat>() ?? other.GetComponentInParent<EnemyPlaneCombat>();

        if (th != null)
        {
            Debug.Log("[Projectile] ✔ Aplicando daño a TargetHealth");
            th.TakeDamage(damage, this.gameObject);
        }

        if (epc != null)
        {
            Debug.Log("[Projectile] ✔ Aplicando daño a EnemyPlaneCombat");
            epc.TakeDamage(damage, this.gameObject);
        }

        // Impacto visual y sonido
        if (impactEffectPrefab != null)
        {
            GameObject fx = Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        if (impactSound != null)
        {
            AudioSource.PlayClipAtPoint(impactSound, transform.position, impactSoundVolume);
        }

        Destroy(gameObject);
    }
}
