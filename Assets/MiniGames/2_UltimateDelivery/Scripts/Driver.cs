using UnityEngine;
using System;

public class Driver : MonoBehaviour
{
    [SerializeField] float steerSpeed = 100f;
    [SerializeField] float carSpeed = 15f;
    [SerializeField] float slowSpeed = 10f;
    [SerializeField] float boostSpeed = 20f;

    // Variables to track button states
    private float horizontalInput = 0f;
    private float verticalInput = 0f;

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
        if (other.tag == "Booster")
        {
            carSpeed = boostSpeed;
        }
    }
}
