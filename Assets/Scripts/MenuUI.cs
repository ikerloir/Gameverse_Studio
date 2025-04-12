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

    void StartWarMode()
    {
        GameManager.Instance.StartWarMode();
    }

    void LoadGameSelect()
    {
        SceneManager.LoadScene("GameSelect");
    }

    void ExitGame()
    {
        Application.Quit();
    }
}