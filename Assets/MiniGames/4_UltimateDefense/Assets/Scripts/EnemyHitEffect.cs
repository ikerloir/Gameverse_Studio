using UnityEngine;

public class EnemyHitEffect : MonoBehaviour, IDamageable
{
    [Header("Effect Settings")]
    public ParticleSystem hitEffectPrefab;  // Prefab de partículas asignado desde el inspector
    public float effectOffset = 0.2f;       // Pequeño desplazamiento hacia afuera para evitar que quede dentro del collider

    public void TakeDamage(float amount, GameObject source)
    {
        if (source == null)
        {
            Debug.LogWarning("[EnemyHitEffect] Source es null. No se puede calcular impacto.");
            return;
        }

        Collider ownCollider = GetComponent<Collider>();
        if (ownCollider == null)
        {
            Debug.LogWarning("[EnemyHitEffect] No tiene Collider asignado.");
            return;
        }

        // Obtener dirección aproximada del proyectil hacia el enemigo
        Vector3 direction = (transform.position - source.transform.position).normalized;

        // Obtener punto de impacto más cercano
        Vector3 hitPoint = ownCollider.ClosestPoint(source.transform.position);

        // Aplicar pequeño desplazamiento hacia afuera del collider en dirección contraria al proyectil
        Vector3 spawnPosition = hitPoint + direction * effectOffset;

        Debug.Log($"[EnemyHitEffect] Daño recibido de: {source.name} | Punto de impacto calculado: {hitPoint} | Posición final con offset: {spawnPosition}");

        // Instanciar el efecto en la posición calculada con rotación neutra
        if (hitEffectPrefab != null)
        {
            var fx = Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);
            Destroy(fx.gameObject, 2f);
            Debug.Log("[EnemyHitEffect] Efecto instanciado correctamente.");
        }
        else
        {
            Debug.LogWarning("[EnemyHitEffect] No se asignó un prefab de efecto.");
        }
    }
}
