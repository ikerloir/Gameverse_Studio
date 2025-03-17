using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    void Start()
    {
        playButton.onClick.AddListener(LoadGameSelect);
        exitButton.onClick.AddListener(ExitGame);
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