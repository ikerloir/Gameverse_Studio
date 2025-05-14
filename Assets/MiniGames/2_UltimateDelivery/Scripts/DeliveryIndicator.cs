using UnityEngine;
using UnityEngine.UI;

public class DeliveryIndicator : MonoBehaviour
{
    [SerializeField] private float edgeBuffer = 50f; // Distance from screen edge
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseScale = 0.2f;

    private Transform targetDeliveryPoint;
    private RectTransform rectTransform;
    private Image indicatorImage;
    private Camera mainCamera;
    private float pulseTime;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        indicatorImage = GetComponent<Image>();
        mainCamera = Camera.main;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (targetDeliveryPoint != null)
        {
            // Get the target position in screen space
            Vector3 targetScreenPos = mainCamera.WorldToScreenPoint(targetDeliveryPoint.position);

            // Check if target is on screen
            bool isOnScreen = targetScreenPos.x > 0 && targetScreenPos.x < Screen.width &&
                             targetScreenPos.y > 0 && targetScreenPos.y < Screen.height &&
                             targetScreenPos.z > 0;

            if (!isOnScreen)
            {
                // Calculate the position on the screen edge
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Vector3 direction = (targetScreenPos - screenCenter).normalized;

                // Calculate the angle for rotation
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);

                // Calculate the position on the screen edge
                float screenWidth = Screen.width;
                float screenHeight = Screen.height;

                // Calculate the intersection with screen edges
                Vector3 screenPos = Vector3.zero;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Intersect with left/right edge
                    float x = direction.x > 0 ? screenWidth - edgeBuffer : edgeBuffer;
                    float y = screenCenter.y + direction.y * (x - screenCenter.x) / direction.x;
                    screenPos = new Vector3(x, Mathf.Clamp(y, edgeBuffer, screenHeight - edgeBuffer), 0f);
                }
                else
                {
                    // Intersect with top/bottom edge
                    float y = direction.y > 0 ? screenHeight - edgeBuffer : edgeBuffer;
                    float x = screenCenter.x + direction.x * (y - screenCenter.y) / direction.y;
                    screenPos = new Vector3(Mathf.Clamp(x, edgeBuffer, screenWidth - edgeBuffer), y, 0f);
                }

                // Update position
                rectTransform.position = screenPos;

                // Pulse effect
                pulseTime += Time.deltaTime * pulseSpeed;
                float scale = 1f + Mathf.Sin(pulseTime) * pulseScale;
                rectTransform.localScale = new Vector3(scale, scale, 1f);

                // Make sure the indicator is visible
                indicatorImage.enabled = true;
            }
            else
            {
                // Hide the indicator when target is on screen
                indicatorImage.enabled = false;
            }
        }
    }

    public void SetTarget(Transform deliveryPoint)
    {
        targetDeliveryPoint = deliveryPoint;
        gameObject.SetActive(deliveryPoint != null);
        if (deliveryPoint == null)
        {
            indicatorImage.enabled = false;
        }
    }
}