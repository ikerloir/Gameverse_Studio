using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zRange = 7f;

    private bool moveLeft = false;
    private bool moveRight = false;
    private float limitLeft = -6.0f;
    private float limiteRigth = 7.0f;

    void Update()
    {
        if (moveLeft)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            if (transform.position.z > zRange)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
            }
               
        }
            

        if (moveRight) {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            if (transform.position.z < -zRange)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -zRange );
            }
                
        }

        //Limites z del jugador

        if (transform.position.z < limitLeft)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limitLeft);
        }

        if (transform.position.z > limiteRigth)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limiteRigth);
        }




    }

   
    public void StartMoveLeft()
    {
        moveLeft = true;
    }

    public void StopMoveLeft()
    {
        moveLeft = false;
    }

    public void StartMoveRight()
    {
        moveRight = true;
    }

    public void StopMoveRight()
    {
        moveRight = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bags"))
        {
            Destroy(other.gameObject);
            Debug.Log("bag recogida");
        }
    }
}

