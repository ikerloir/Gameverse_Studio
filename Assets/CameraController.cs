using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Canvas targetCanvas; // Referencia al Canvas que la c�mara debe enfocar

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

        // Asegurar que la c�mara est� en modo Ortogr�fico para UI
        Camera cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError(" No se encontr� una c�mara en " + gameObject.name);
            return;
        }

        cam.orthographic = true; // Activar vista ortogr�fica

        // Obtener el tama�o del Canvas y ajustarlo a la c�mara
        RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
        if (canvasRect != null)
        {
            float canvasHeight = canvasRect.sizeDelta.y;
            cam.orthographicSize = canvasHeight / 2;
        }

        // Asegurar que la c�mara est� centrada en el Canvas
        transform.position = new Vector3(targetCanvas.transform.position.x, targetCanvas.transform.position.y, -10);
    }
}
