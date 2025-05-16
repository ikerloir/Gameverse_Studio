using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button jugarGerraTotalButton;
    public Button jugarJuegoIndividualButton;
    public Button exitButton;

    void Start()
    {
        jugarGerraTotalButton.onClick.AddListener(StartWarMode);
        jugarJuegoIndividualButton.onClick.AddListener(LoadGameSelect);
        exitButton.onClick.AddListener(ExitGame);
    }
    // modo guerra
    void StartWarMode()
    {
        GameManager.Instance.StartWarMode();
    }
    // carga scena GameSelect
    void LoadGameSelect()
    {
        SceneManager.LoadScene("GameSelect");
    }
    // cerrar juego
    void ExitGame()
    {
        Application.Quit();
    }
}