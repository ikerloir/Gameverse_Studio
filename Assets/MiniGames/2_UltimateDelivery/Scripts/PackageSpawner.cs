using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private Transform spawnPoint;
    private GameObject currentPackage;

    private void Start()
    {
        SpawnPackage();
    }

    public void SpawnPackage()
    {
        if (currentPackage == null)
        {
            currentPackage = Instantiate(packagePrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void PackagePickedUp()
    {
        currentPackage = null;
    }
}