using System.Collections;
using TMPro;
using UnityEngine;

public class IntroUIManager : MonoBehaviour
{
    [Header("Texto de introducci�n")]
    public TextMeshProUGUI introText;
    [TextArea]
    public string mensaje = "Nos est�n atacando. �Defiende el aeropuerto!";

    [Header("Velocidad de m�quina de escribir (letras/segundo)")]
    public float typingSpeed = 30f;

    [Header("Duraci�n despu�s de escribir")]
    public float holdDuration = 2f;

    [Header("Sonido de m�quina por letra")]
    public AudioSource typingSound;

    [Header("Objetos que se activan despu�s de la intro")]
    public GameObject[] objectsToActivate;

    private void Start()
    {
        if (introText != null)
        {
            introText.gameObject.SetActive(true);
            introText.text = "";
            StartCoroutine(PlayIntro());
        }
        else
        {
            Debug.LogWarning("No se ha asignado un TextMeshProUGUI al IntroUIManager.");
        }
    }

    IEnumerator PlayIntro()
    {
        // M�quina de escribir letra por letra
        for (int i = 0; i < mensaje.Length; i++)
        {
            introText.text += mensaje[i];

            if (typingSound != null && mensaje[i] != ' ')
                typingSound.Play();

            yield return new WaitForSeconds(1f / typingSpeed);
        }

        // Esperamos
        yield return new WaitForSeconds(holdDuration);

        // Fade-out suave
        float fadeDuration = 1f;
        float elapsed = 0f;
        Color originalColor = introText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            introText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        introText.gameObject.SetActive(false);

        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
