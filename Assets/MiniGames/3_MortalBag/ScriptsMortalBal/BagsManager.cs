using UnityEngine;

public class BagsManager : MonoBehaviour
{
    public GameObject[] bagsPrefabs;
    private float startDelay = 2;
    public float spawnInterval = 2.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnRandomBags", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnRandomBags()
    {
        int bagsIndex = Random.Range(0, bagsPrefabs.Length);
        int bagPos = Random.Range(0, 4);
        //Vector3 spwanPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnRangeX);
        switch (bagPos)
        {
            case 0:
                Vector3 spawnPos1 = new Vector3(-11.059f, 2, 12);
                Instantiate(bagsPrefabs[bagsIndex], spawnPos1, bagsPrefabs[bagsIndex].transform.rotation);
                break;
            case 1:
                Vector3 spawnPos2 = new Vector3(-11.059f, 4.28f, -11.25f);
                Instantiate(bagsPrefabs[bagsIndex], spawnPos2, bagsPrefabs[bagsIndex].transform.rotation);
                break;
            case 2:
                Vector3 spawnPos3 = new Vector3(-11.059f, 3.76f, 12.22f);
                Instantiate(bagsPrefabs[bagsIndex], spawnPos3, bagsPrefabs[bagsIndex].transform.rotation);
                break;
            case 3:
                Vector3 spawnPos4 = new Vector3(-11.059f, 2, -11.44f);
                Instantiate(bagsPrefabs[bagsIndex], spawnPos4, bagsPrefabs[bagsIndex].transform.rotation);
                break;
            default:
                break;
        }

    }
}
