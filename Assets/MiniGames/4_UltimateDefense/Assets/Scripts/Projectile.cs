using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float speed = 100f;
    public float damage = 10f;
    public string targetTag = "Enemy";
    public string impactEffectTag = "Impact";

    private Vector3 direction;
    private bool hasHit = false;

    private static HUDManager hud;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        hasHit = false;
    }

    void Start()
    {
        if (hud == null)
        {
            hud = FindObjectOfType<HUDManager>();
        }

        Invoke(nameof(Deactivate), 5f);
    }

    void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit || (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag)))
            return;

        bool acertado = false;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage, gameObject);
            acertado = true;
        }

        hasHit = true;

        if (targetTag == "Enemy" && hud != null)
        {
            hud.AddShot(acertado);
        }

        if (!string.IsNullOrEmpty(impactEffectTag))
        {
            Vector3 hitPos = other.ClosestPoint(transform.position);
            GameObject fx = ObjectPooler.Instance.SpawnFromPool(impactEffectTag, hitPos, Quaternion.LookRotation(-direction));
            StartCoroutine(DisableAfterSeconds(fx, 2f));
        }

        Deactivate();
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DisableAfterSeconds(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }
}
