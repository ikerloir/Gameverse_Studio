using UnityEngine;

public class Package : MonoBehaviour
{
    private void Start()
    {
        // Make sure the package has a trigger collider
        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().isTrigger = true;
        }
        else
        {
            Debug.LogError("Package needs a Collider2D component set as trigger!");
        }
    }
}