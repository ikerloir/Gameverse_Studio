using UnityEngine;

public class BagCollision : MonoBehaviour
{
    public int points = 1;
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
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
            Debug.Log("Maleta caída al suelo");
            if (MortalBagHealthManager.Instance != null)
            {
                MortalBagHealthManager.Instance.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}