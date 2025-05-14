using UnityEngine;
using System.Collections.Generic;

public class DeliveryPointManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> deliveryPoints = new List<GameObject>();
    [SerializeField] private DeliveryIndicator deliveryIndicator;
    private int currentActivePoint = -1;

    private void Start()
    {
        // Deactivate all delivery points at start
        foreach (var point in deliveryPoints)
        {
            point.SetActive(false);
        }
    }

    public void ActivateRandomDeliveryPoint()
    {
        // Deactivate current point if there is one
        if (currentActivePoint != -1)
        {
            deliveryPoints[currentActivePoint].SetActive(false);
        }

        // Choose a new random point
        int newPoint;
        do
        {
            newPoint = Random.Range(0, deliveryPoints.Count);
        } while (newPoint == currentActivePoint && deliveryPoints.Count > 1);

        currentActivePoint = newPoint;
        deliveryPoints[currentActivePoint].SetActive(true);

        // Update the delivery indicator
        if (deliveryIndicator != null)
        {
            deliveryIndicator.SetTarget(deliveryPoints[currentActivePoint].transform);
        }
    }

    public void DeactivateCurrentPoint()
    {
        if (currentActivePoint != -1)
        {
            deliveryPoints[currentActivePoint].SetActive(false);
            // Hide the delivery indicator
            if (deliveryIndicator != null)
            {
                deliveryIndicator.SetTarget(null);
            }
            currentActivePoint = -1;
        }
    }
}