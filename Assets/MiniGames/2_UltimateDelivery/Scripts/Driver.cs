using UnityEngine;
using System;

public class Driver : MonoBehaviour
{
    [SerializeField] float steerSpeed = 100f;
    [SerializeField] float carSpeed = 15f;
    [SerializeField] float slowSpeed = 10f;
    [SerializeField] float boostSpeed = 20f;
    [SerializeField] Color32 hasPackageColor = new Color32(255, 94, 222, 255);
    [SerializeField] Color32 noPackageColor = new Color32(0, 255, 254, 255);

    // Variables to track button states
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private bool hasPackage = false;
    private SpriteRenderer spriteRenderer;
    private DeliveryPointManager deliveryPointManager;
    private PackageSpawner packageSpawner;
    private DeliveryScoreManager scoreManager;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = noPackageColor;
        deliveryPointManager = FindFirstObjectByType<DeliveryPointManager>();
        packageSpawner = FindFirstObjectByType<PackageSpawner>();
        scoreManager = FindFirstObjectByType<DeliveryScoreManager>();
    }

    void Update()
    {
        float keyboardHorizontal = Input.GetAxis("Horizontal");
        float keyboardVertical = Input.GetAxis("Vertical");

        float steerAmount = -1 * (Mathf.Abs(keyboardHorizontal) > Mathf.Abs(horizontalInput) ?
                                keyboardHorizontal : horizontalInput) * steerSpeed * Time.deltaTime;

        float moveAmount = (Mathf.Abs(keyboardVertical) > Mathf.Abs(verticalInput) ?
                          keyboardVertical : verticalInput) * carSpeed * Time.deltaTime;

        transform.Rotate(0, 0, steerAmount);
        transform.Translate(0.0f, moveAmount, 0.0f);
    }

    // Methods for touch buttons
    public void OnLeftButtonDown() { horizontalInput = -1f; }
    public void OnLeftButtonUp() { horizontalInput = 0f; }

    public void OnRightButtonDown() { horizontalInput = 1f; }
    public void OnRightButtonUp() { horizontalInput = 0f; }

    public void OnForwardButtonDown() { verticalInput = 1f; }
    public void OnForwardButtonUp() { verticalInput = 0f; }

    public void OnBackButtonDown() { verticalInput = -1f; }
    public void OnBackButtonUp() { verticalInput = 0f; }

    private void OnCollisionEnter2D(Collision2D other)
    {
        carSpeed = slowSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Booster"))
        {
            carSpeed = boostSpeed;
        }
        else if (other.CompareTag("Package") && !hasPackage)
        {
            Debug.Log("Package picked up");
            hasPackage = true;
            spriteRenderer.color = hasPackageColor;
            other.gameObject.SetActive(false);
            packageSpawner.PackagePickedUp();
            // Activate a random delivery point when package is picked up
            deliveryPointManager.ActivateRandomDeliveryPoint();
        }
        else if (other.CompareTag("Customer") && hasPackage)
        {
            Debug.Log("Package delivered");
            hasPackage = false;
            spriteRenderer.color = noPackageColor;
            // Deactivate the current delivery point
            deliveryPointManager.DeactivateCurrentPoint();
            // Spawn a new package
            packageSpawner.SpawnPackage();
            // Add a point to the score
            scoreManager.AddPoint();
        }
    }
}
