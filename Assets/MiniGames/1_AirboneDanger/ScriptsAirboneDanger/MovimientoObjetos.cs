using UnityEngine;

public class MovimientoObjetos : MonoBehaviour
{
    public float speed = 1.0f;
    private Rigidbody objectRb;
    private float zBound = 30.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        objectRb.AddForce(Vector3.forward * -speed);

        if (transform.position.z < zBound)
        {
            Destroy(gameObject);
        }
    }
}
