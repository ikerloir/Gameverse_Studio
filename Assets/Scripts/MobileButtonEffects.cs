using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class MobileButtonEffects : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Button button;
    private Image image;

    public Color normalColor = Color.white;
    public Color pressedColor = new Color(0.9f, 0.6f, 0.2f);
    public float pressedScale = 0.95f;
    public float animationSpeed = 0.08f;

    [Header("Opcionales")]
    public AudioClip clickSound;
    public ParticleSystem clickParticles;

    private AudioSource audioSource;

    void Awake()
    {
        originalScale = transform.localScale;
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.color = normalColor;

        if (clickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = clickSound;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateButton(originalScale * pressedScale, pressedColor));

        if (audioSource != null)
            audioSource.Play();

        if (clickParticles != null)
            Instantiate(clickParticles, transform.position, Quaternion.identity, transform);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateButton(originalScale, normalColor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateButton(originalScale, normalColor));
    }

    private IEnumerator AnimateButton(Vector3 targetScale, Color targetColor)
    {
        Vector3 currentScale = transform.localScale;
        Color currentColor = image.color;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / animationSpeed;
            transform.localScale = Vector3.Lerp(currentScale, targetScale, t);
            image.color = Color.Lerp(currentColor, targetColor, t);
            yield return null;
        }

        transform.localScale = targetScale;
        image.color = targetColor;
    }
}
