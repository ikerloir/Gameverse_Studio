using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Canvas targetCanvas; // Referencia al Canvas que la cámara debe enfocar

    private void Start()
    {
        AdjustCameraToCanvas();
    }

    void AdjustCameraToCanvas()
    {
        if (targetCanvas == null)
        {
            Debug.LogError(" No se ha asignado un Canvas en " + gameObject.name);
            return;
        }

        // Asegurar que la cámara está en modo Ortográfico para UI
        Camera cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError(" No se encontró una cámara en " + gameObject.name);
            return;
        }

        cam.orthographic = true; // Activar vista ortográfica

        // Obtener el tamaño del Canvas y ajustarlo a la cámara
        RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
        if (canvasRect != null)
        {
            float canvasHeight = canvasRect.sizeDelta.y;
            cam.orthographicSize = canvasHeight / 2;
        }

        // Asegurar que la cámara está centrada en el Canvas
        transform.position = new Vector3(targetCanvas.transform.position.x, targetCanvas.transform.position.y, -10);
    }
}
