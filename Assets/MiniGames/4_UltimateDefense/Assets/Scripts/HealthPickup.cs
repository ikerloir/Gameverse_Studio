using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float extraLife = 50f;
    public AudioClip pickupSound;
    public float pickupSoundVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        UnifiedDamageReceiver playerHealth = other.GetComponent<UnifiedDamageReceiver>();
        if (playerHealth != null)
        {
            // Curar usando el mismo método de daño con valor negativo
            playerHealth.TakeDamage(-extraLife, this.gameObject);

            // Reproducir sonido
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, pickupSoundVolume);

            // Informar a HUDManager (opcional, si quieres mostrar "vida recogida")
            HUDManager hud = FindFirstObjectByType<HUDManager>();

            // Destruir el pickup
            Destroy(gameObject);
        }
    }
}
