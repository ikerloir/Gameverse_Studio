using UnityEngine;

public class EnemyPlaneAI : MonoBehaviour
{
    public float speed = 30f;
    public float rotationSpeed = 2f;
    public BoxCollider flightZone;

    [Header("Altura controlada")]
    public float minY = 1.2f; // altura mínima (ideal para persona de 1.70m)
    public float maxY = 2.5f; // altura máxima visible en AR

    [Header("Cambio de rumbo aleatorio")]
    public float changeTargetInterval = 4f;
    private float changeTargetTimer = 0f;

    private Vector3 currentTarget;

    void Start()
    {
        if (flightZone == null)
        {
            Debug.LogWarning("No se asignó la zona de vuelo al EnemyPlaneAI.");
            enabled = false;
            return;
        }

        // Posición inicial 2 metros frente a la cámara, a ~1.7m de altura, en diagonal
        Vector3 initialOffset = Camera.main.transform.forward * 2f + Vector3.up * 1.7f;
        transform.position = Camera.main.transform.position + initialOffset;
        transform.rotation = Quaternion.LookRotation(Vector3.down + Camera.main.transform.forward);

        PickNewRandomTarget();
    }

    void Update()
    {
        changeTargetTimer -= Time.deltaTime;

        // Cambiar de destino aleatoriamente cada cierto tiempo
        if (changeTargetTimer <= 0f || Vector3.Distance(transform.position, currentTarget) < 1f)
        {
            PickNewRandomTarget();
            changeTargetTimer = changeTargetInterval;
        }

        MoveTowards(currentTarget);
    }

    void MoveTowards(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void PickNewRandomTarget()
    {
        if (flightZone == null) return;

        Bounds bounds = flightZone.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(minY, maxY); // Altura controlada
        float z = Random.Range(bounds.min.z, bounds.max.z);

        currentTarget = new Vector3(x, y, z);
    }
}
