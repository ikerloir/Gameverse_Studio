using UnityEngine;

public class EnemyHitEffect : MonoBehaviour, IDamageable
{
    [Header("Effect Settings")]
    public ParticleSystem hitEffectPrefab;  // Prefab de part�culas asignado desde el inspector
    public float effectOffset = 0.2f;       // Peque�o desplazamiento hacia afuera para evitar que quede dentro del collider

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

        // Obtener direcci�n aproximada del proyectil hacia el enemigo
        Vector3 direction = (transform.position - source.transform.position).normalized;

        // Obtener punto de impacto m�s cercano
        Vector3 hitPoint = ownCollider.ClosestPoint(source.transform.position);

        // Aplicar peque�o desplazamiento hacia afuera del collider en direcci�n contraria al proyectil
        Vector3 spawnPosition = hitPoint + direction * effectOffset;

        Debug.Log($"[EnemyHitEffect] Da�o recibido de: {source.name} | Punto de impacto calculado: {hitPoint} | Posici�n final con offset: {spawnPosition}");

        // Instanciar el efecto en la posici�n calculada con rotaci�n neutra
        if (hitEffectPrefab != null)
        {
            var fx = Instantiate(hitEffectPrefab, spawnPosition, Quaternion.identity);
            Destroy(fx.gameObject, 2f);
            Debug.Log("[EnemyHitEffect] Efecto instanciado correctamente.");
        }
        else
        {
            Debug.LogWarning("[EnemyHitEffect] No se asign� un prefab de efecto.");
        }
    }
}
