using UnityEngine;

public class RotarHelice : MonoBehaviour
{
    [Header("Velocidad de rotación (grados por segundo)")]
    public float velocidad = 1000f;

    [Header("Eje de rotación")]
    public Vector3 eje = Vector3.forward; // Z-axis por defecto

    void Update()
    {
        transform.Rotate(eje * velocidad * Time.deltaTime);
    }
}
