using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemigosPrefabs; 
    public GameObject gasolinaPrefab;
    private float spawnX = 6.0f;
    private float posZ = 100.0f;
    private float spawnPosUpY = 6.0f;
    private float spawnPosDownY = 1.6f;
    private float delayTime = 5.0f;
    private float repitTimeGasolina = 7.0f;
    public float repitTimeEnemigos;
    private GameManagerAirboneDanger gameManagerAirboneDanger;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManagerAirboneDanger = GameObject.Find("GameManagerAirboneDanger").GetComponent<GameManagerAirboneDanger>();    
        
        InvokeRepeating("SpawnGasolina", delayTime, repitTimeGasolina);
        InvokeRepeating("SpawnEnemigos", delayTime, repitTimeEnemigos);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnGasolina()
    {
        if (gameManagerAirboneDanger.isGameActive)
        {
            float randomGasolinaX = Random.Range(-spawnX, spawnX);
            float randomGasolinay = Random.Range(spawnPosDownY, spawnPosUpY);
            Vector3 spawnGasolinaPos = new Vector3(randomGasolinaX, randomGasolinay, posZ);

            Instantiate(gasolinaPrefab, spawnGasolinaPos, gasolinaPrefab.transform.rotation);
        }
    }

    void SpawnEnemigos()
    {
        if (gameManagerAirboneDanger.isGameActive)
        {


            int indexEnemigo = Random.Range(0, 3);
            float randomEnemigoX = Random.Range(-spawnX, spawnX);
            float randomEnemigoY = Random.Range(spawnPosDownY, spawnPosUpY);
            Vector3 spawnEnemigoPos = new Vector3(randomEnemigoX, randomEnemigoY, posZ);

            Instantiate(enemigosPrefabs[indexEnemigo], spawnEnemigoPos, enemigosPrefabs[indexEnemigo].transform.rotation);
        }
    }
}
