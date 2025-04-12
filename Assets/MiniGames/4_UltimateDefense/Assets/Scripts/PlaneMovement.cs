using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    [Header("Destinos")]
    public Transform[] waypoints;  // Lista de puntos por los que debe pasar el avión
    private int currentWaypointIndex = 0;

    [Header("Movimiento General")]
    public float speed = 5f; // Velocidad de traslación
    public float rotationSpeed = 2f; // Velocidad de giro suave
    public float waypointThreshold = 1f; // Distancia mínima para cambiar al siguiente waypoint

    [Header("Órbita y Altitud")]
    public float baseAltitude = 10f;
    public float altitudeVariation = 3f;
    public float altitudeSpeed = 1f;

    [Header("Oscilaciones y Giros")]
    public float rollAmount = 20f;
    public float rollSpeed = 2f;
    public float yawOscillation = 10f;

    [Header("Zona de Vuelo")]
    public BoxCollider flightZone; // Asignar zona en la que el avión puede volar

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform currentWaypoint = waypoints[currentWaypointIndex];
        Transform nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Length];

        // Calcular posición de destino con altura oscilante
        Vector3 targetPosition = new Vector3(
            currentWaypoint.position.x,
            currentWaypoint.position.y + baseAltitude + Mathf.Sin(Time.time * altitudeSpeed) * altitudeVariation,
            currentWaypoint.position.z
        );

        // Mover hacia el destino
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Calcular dirección hacia el siguiente waypoint para orientar el avión
        Vector3 directionToNext = (nextWaypoint.position - transform.position).normalized;
        if (directionToNext != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToNext);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Oscilaciones naturales (roll y yaw)
        float rollAngle = Mathf.Sin(Time.time * rollSpeed) * rollAmount;
        float yawAngle = Mathf.Sin(Time.time * rollSpeed) * yawOscillation;
        transform.Rotate(Vector3.forward, rollAngle * Time.deltaTime);
        transform.Rotate(Vector3.up, yawAngle * Time.deltaTime);

        // Avanzar al siguiente waypoint si estamos lo suficientemente cerca
        float distanceToCurrent = Vector3.Distance(transform.position, currentWaypoint.position);
        if (distanceToCurrent < waypointThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // 🔒 Verificación de zona de vuelo
        if (flightZone != null && !flightZone.bounds.Contains(transform.position))
        {
            Vector3 center = flightZone.bounds.center;
            transform.position = Vector3.MoveTowards(transform.position, center, speed * Time.deltaTime);
        }
    }
}
