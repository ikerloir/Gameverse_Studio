using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public Button introBoton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
