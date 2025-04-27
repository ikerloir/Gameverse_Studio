using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogoIntro : MonoBehaviour
{
    public Image logoImage; // La imagen del logo
    public AudioSource introMusic; // Audio de la intro
    public float fadeInDuration = 2f; // Duración del fade-in
    public float shakeDuration = 1f; // Duración del temblor
    public float shakeIntensity = 2f; // Intensidad del temblor

    private Vector3 originalPosition;

    void Start()
    {
        // Guardar la posición inicial del logo
        originalPosition = logoImage.transform.position;

        // Hacer la imagen completamente transparente al inicio
        Color startColor = logoImage.color;
        startColor.a = 0;
        logoImage.color = startColor;

        // Iniciar la secuencia de efectos
        StartCoroutine(PlayIntroEffects());
    }

    IEnumerator PlayIntroEffects()
    {
        // Reproducir música de la intro
        if (introMusic != null)
        {
            introMusic.Play();
        }

        // Efecto de Fade-In
        float elapsedTime = 0;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = elapsedTime / fadeInDuration;
            Color newColor = logoImage.color;
            newColor.a = alpha;
            logoImage.color = newColor;
            yield return null;
        }

        // Asegurar que el logo quede completamente visible
        Color finalColor = logoImage.color;
        finalColor.a = 1;
        logoImage.color = finalColor;

        // Efecto de temblor
        yield return StartCoroutine(ShakeEffect());
    }

    IEnumerator ShakeEffect()
    {
        float elapsedTime = 0;
        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity) * (1 - (elapsedTime / shakeDuration));
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity) * (1 - (elapsedTime / shakeDuration));
            logoImage.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);
            yield return null;
        }

        // Restaurar la posición original
        logoImage.transform.position = originalPosition;
    }
}
