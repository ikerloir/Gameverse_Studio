using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 10f); // Se destruye luego de 1 segundo
    }
}
