using UnityEngine;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1.5f;
    public TextMeshPro text;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    public void SetDamage(float amount)
    {
        if (text != null)
            text.text = "-" + amount.ToString("F0");
    }
}
