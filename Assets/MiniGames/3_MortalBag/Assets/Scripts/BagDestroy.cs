using UnityEngine;

public class BagDestroy : MonoBehaviour
{
    public float yLimit = -2.0f;

    void Update()
    {
        
        if (transform.position.y < yLimit)
        {
            Destroy(gameObject);
        }
    }
}
