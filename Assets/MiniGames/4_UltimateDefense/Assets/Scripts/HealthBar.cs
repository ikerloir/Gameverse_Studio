using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform target;            // Objetivo (enemigo)
    public Vector3 offset = new Vector3(0, 2f, 0);  // Offset sobre la cabeza
    public float maxWidth = 1.5f;       // Ancho máximo de la barra

    private GameObject barObject;
    private Material barMaterial;

    void Start()
    {
        // Crear el quad para la barra
        barObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        barObject.transform.SetParent(transform);
        barObject.transform.localPosition = Vector3.zero;
        barObject.transform.localRotation = Quaternion.identity;
        barObject.transform.localScale = new Vector3(maxWidth, 0.2f, 1f);

        // Desactivar colisionador
        Destroy(barObject.GetComponent<Collider>());

        // Crear material básico
        barMaterial = new Material(Shader.Find("Unlit/Color"));
        barMaterial.color = Color.green;
        barObject.GetComponent<MeshRenderer>().material = barMaterial;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Seguir al objetivo
        transform.position = target.position + offset;

        // Mirar a la cámara
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0); // Corregir orientación
    }

    /// <summary>
    /// Actualiza la barra según la salud
    /// </summary>
    public void SetHealth(float current, float max)
    {
        float ratio = Mathf.Clamp01(current / max);

        // Escalar en X
        if (barObject != null)
        {
            Vector3 scale = barObject.transform.localScale;
            scale.x = maxWidth * ratio;
            barObject.transform.localScale = scale;
        }

        // Cambiar color dinámicamente
        if (barMaterial != null)
        {
            if (ratio > 0.6f)
                barMaterial.color = Color.green;
            else if (ratio > 0.3f)
                barMaterial.color = Color.yellow;
            else
                barMaterial.color = Color.red;
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
}
