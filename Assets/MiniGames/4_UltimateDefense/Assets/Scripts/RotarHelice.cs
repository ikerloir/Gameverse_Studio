using UnityEngine;

public class RotarHelice : MonoBehaviour
{
    [Header("Velocidad de rotaci�n (grados por segundo)")]
    public float velocidad = 1000f;

    [Header("Eje de rotaci�n")]
    public Vector3 eje = Vector3.forward; // Z-axis por defecto

    void Update()
    {
        transform.Rotate(eje * velocidad * Time.deltaTime);
    }
}
