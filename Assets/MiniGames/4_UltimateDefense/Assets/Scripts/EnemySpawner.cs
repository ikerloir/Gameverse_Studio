using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField] private GameObject enemyPrefabRoot;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private bool autoSpawn = false;
    [SerializeField] private bool debugLogs = true;

    [Header("Spawn Position Range")]
    [SerializeField] private float rangeX = 10f;
    [SerializeField] private float rangeZ = 20f;
    [SerializeField] private float heightY = 2f;

    private float timer;

    void Update()
    {
        if (autoSpawn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SpawnEnemy();
                timer = spawnInterval;
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (enemyPrefabRoot == null)
        {
            Debug.LogWarning("❌ EnemySpawner: No hay prefab asignado.");
            return;
        }

        Vector3 spawnPos = new Vector3(
            Random.Range(-rangeX, rangeX),
            heightY,
            Random.Range(10f, rangeZ)
        );

        GameObject enemy = Instantiate(enemyPrefabRoot, spawnPos, Quaternion.identity);

        if (debugLogs)
        {
            Debug.Log($"🛫 Spawned enemy at {spawnPos}");
        }
    }
}