using UnityEngine;

public class EnemyPlaneAI : MonoBehaviour
{
    public float speed = 30f;
    public float rotationSpeed = 2f;
    public BoxCollider flightZone;

    [Header("Altura controlada")]
    public float minY = 1.2f;
    public float maxY = 2.5f;

    [Header("Cambio de rumbo aleatorio")]
    public float changeTargetInterval = 4f;
    private float nextTargetChangeTime = 0f;

    private Vector3 currentTarget;
    private Transform camTransform;

    void Start()
    {
        if (flightZone == null)
        {
            Debug.LogWarning("No se asignó la zona de vuelo al EnemyPlaneAI.");
            enabled = false;
            return;
        }

        camTransform = Camera.main.transform;

        // Posición inicial
        Vector3 initialOffset = camTransform.forward * 2f + Vector3.up * 1.7f;
        transform.position = camTransform.position + initialOffset;
        transform.rotation = Quaternion.LookRotation(Vector3.down + camTransform.forward);

        PickNewRandomTarget();
        nextTargetChangeTime = Time.time + changeTargetInterval;
    }

    void Update()
    {
        if (Time.time >= nextTargetChangeTime || (transform.position - currentTarget).sqrMagnitude < 1f)
        {
            PickNewRandomTarget();
            nextTargetChangeTime = Time.time + changeTargetInterval;
        }
    }

    void FixedUpdate()
    {
        MoveTowards(currentTarget);
    }

    void MoveTowards(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }

    void PickNewRandomTarget()
    {
        Bounds bounds = flightZone.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(minY, maxY);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        currentTarget = new Vector3(x, y, z);
    }
}
