using UnityEngine;

public class DebugOnHit : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[PruebaColision] Colisi�n detectada con: {collision.gameObject.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[PruebaColision] Trigger activado por: {other.gameObject.name}");
    }
}
