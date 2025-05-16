using Unity.VisualScripting;
using UnityEngine;

public class SeguimientoCamara : MonoBehaviour

{
    public Transform avion;
    private Vector3 offset = new Vector3(0,2,-10);
    private bool seguirAvion = true; // Controla si la cámara debe seguir al avión
    private Camera cam;

    private Animation avionAnimation;

    private void Start()
    {
        avionAnimation = avion.GetComponent<Animation>(); // Obtener el Animator del avión
        cam = GetComponent<Camera>();
        
    }

    void Update()
    {
        // Verificar si la animación de despegue ha terminado
        if (avionAnimation != null)
        {
            if (!avionAnimation.isPlaying || !avionAnimation.IsPlaying("Despegue"))
            {
                seguirAvion = false; // Dejar de seguir al avión cuando la animación termine
            }
        }
    }
    void LateUpdate()
    {
        // Solo sigue al avión si el despegue está en progreso
        if (seguirAvion)
        {
            transform.position = avion.position + offset;
            transform.LookAt(avion);
        }
        
        else
        {
            // Detiene el movimiento de la cámara una vez que el avión terminó el despegue
            transform.position = transform.position;
            transform.rotation = Quaternion.Euler(-7, 0, 0);
            
        }
    }
}


