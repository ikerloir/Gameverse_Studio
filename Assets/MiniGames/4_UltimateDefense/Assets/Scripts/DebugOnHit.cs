using UnityEngine;

public class DebugOnHit : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[PruebaColision] Colisión detectada con: {collision.gameObject.name}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[PruebaColision] Trigger activado por: {other.gameObject.name}");
    }
}
