using UnityEngine;

public class MovimientoObjetos : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody objectRb;
    private float zBound = 30.0f;
   
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
    }

    // mueve los objetos y los destruyen cuando estan fuera de la pantalla
    void Update()
    {
        objectRb.AddForce(Vector3.forward * -speed);

        if (transform.position.z < zBound)
        {
            Destroy(gameObject);
        }
    }
}
