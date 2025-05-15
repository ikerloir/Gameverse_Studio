using UnityEngine;

public class SpawnNewEnemy : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public void SpawnNewEnemyMethod()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-10f, 10f),
            transform.position.y,
            Random.Range(15f, 25f)
        );

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        float nuevaVida = 100f;
        float nuevoDaño = 1f;

        if (newEnemy.TryGetComponent(out EnemyPlaneCombat comp))
        {
            comp.maxHealth = nuevaVida;
            comp.baseDamage = nuevoDaño;
        }

        if (newEnemy.TryGetComponent(out UnifiedDamageReceiver hp))
        {
            var field = hp.GetType().GetField("maxHealth");
            if (field != null) field.SetValue(hp, nuevaVida);
        }

        Debug.Log($"🛫 [Spawn] Enemigo creado: Vida = {nuevaVida}, Daño = {nuevoDaño}");
    }
}
