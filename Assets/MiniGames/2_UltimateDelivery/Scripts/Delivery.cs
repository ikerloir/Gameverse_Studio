using Unity.VisualScripting;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] float destroyDelay = 1.0f;
    [SerializeField] Color32 hasPackageColor = new Color32(255, 94, 222, 255);
    [SerializeField] Color32 noPackageColor = new Color32(0, 255, 254, 255);

    private bool havePackage;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Ups, I crashed the car again");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Package" && !havePackage)
        {
            Debug.Log("Package picked up");
            havePackage = true;
            spriteRenderer.color = hasPackageColor;
            Destroy(other.gameObject, destroyDelay);
        }
        else if (other.tag == "Customer" && havePackage)
        {
            Debug.Log("Package has been Delivered");
            havePackage = false;
            spriteRenderer.color = noPackageColor;
        }
    }
}
