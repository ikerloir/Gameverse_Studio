using UnityEngine;

public class BagCollision : MonoBehaviour
{
    public int points = 1;
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Colisión detectada con: {other.gameObject.name}, Tag: {other.tag}");

        // Si colisiona con el jugador
        if (other.CompareTag("Player"))
        {
            Debug.Log("Maleta recogida por el jugador");
            if (MortalBagScoreManager.Instance != null)
            {
                MortalBagScoreManager.Instance.AddScore(points);
            }
            Destroy(gameObject);
        }
        // Si colisiona con el suelo
        else if (other.CompareTag("Ground"))
        {
            Debug.Log("Maleta caída al suelo - Intentando quitar vida");
            if (MortalBagHealthManager.Instance != null)
            {
                Debug.Log("HealthManager encontrado, quitando vida");
                MortalBagHealthManager.Instance.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("MortalBagHealthManager.Instance es null. Asegúrate de que existe en la escena.");
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Colisión con objeto no reconocido: {other.gameObject.name} con tag {other.tag}");
        }
    }
}