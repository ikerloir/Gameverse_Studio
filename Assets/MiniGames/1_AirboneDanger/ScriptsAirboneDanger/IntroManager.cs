using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public Button introBoton;
    
    // carga el texto de la intro y pasado unos segundos muestra el boton de despegue que carga la escnea del juego
    void Start()
    {
        introText.gameObject.SetActive(true);
        introBoton.gameObject.SetActive(false);
        StartCoroutine(Intro());
    }

    private IEnumerator Intro()
    {
        yield return new WaitForSeconds(3.0f);

        introBoton.gameObject.SetActive(true);
        introBoton.onClick.AddListener(() => SceneManager.LoadScene("AirboneDanger"));

    }
}
