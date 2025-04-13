using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [Header("Configuración de vida")]
    public int maxLife = 100;
    private int currentLife;

    [Header("HUD")]
    public LifeHUD lifeHUD; // Script que actualiza la vida en pantalla

    [Header("Efectos de daño")]
    public CameraShake cameraShake; // Script que maneja la vibración
    public AudioClip explosionSound;
    public AudioSource audioSource;

    void Start()
    {
        currentLife = maxLife;

        if (lifeHUD != null)
            lifeHUD.UpdateLife(currentLife);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LP"))
        {
            Debug.Log("💥 Impacto de proyectil LP");

            // Reducir vida (por ejemplo, 10 puntos)
            int damage = 10;
            currentLife -= damage;
            currentLife = Mathf.Max(0, currentLife);

            // Actualizar HUD
            if (lifeHUD != null)
                lifeHUD.UpdateLife(currentLife);

            // Vibrar cámara
            if (cameraShake != null)
                cameraShake.TriggerShake();

            // Reproducir sonido
            if (audioSource != null && explosionSound != null)
                audioSource.PlayOneShot(explosionSound);

            // Destruir el proyectil
            Destroy(collision.gameObject);
        }
    }
}
