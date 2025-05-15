using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    public int points = 1; // Cambiado a 1 punto por maleta

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Colisión detectada con: {other.gameObject.name}, Tag: {other.tag}");

        // Verificar si la colisión es con una maleta
        if (other.CompareTag("bags"))
        {
            Debug.Log("Colisión con maleta detectada");

            if (MortalBagScoreManager.Instance != null)
            {
                Debug.Log("MortalBagScoreManager encontrado, intentando sumar puntos");
                MortalBagScoreManager.Instance.AddScore(points);
            }
            else
            {
                Debug.LogError("MortalBagScoreManager.Instance es null. Asegúrate de que existe en la escena.");
            }

            // Destruir la maleta después de la colisión
            Debug.Log("Destruyendo la maleta");
            Destroy(other.gameObject);
        }
    }
}
