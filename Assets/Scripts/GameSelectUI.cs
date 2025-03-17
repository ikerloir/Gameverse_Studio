using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSelectUI : MonoBehaviour
{
    public Button game1Button;
    public Button game2Button;
    public Button game3Button;
    public Button gameVRButton;
    public Button gameARButton;
    public Button backButton;

    void Start()
    {
        game1Button.onClick.AddListener(() => LoadGame("Game3DA"));
        game2Button.onClick.AddListener(() => LoadGame("Game3DB"));
        game3Button.onClick.AddListener(() => LoadGame("Game2D"));
        gameVRButton.onClick.AddListener(() => LoadGame("GameVR"));
        gameARButton.onClick.AddListener(() => LoadGame("GameAR"));
        backButton.onClick.AddListener(LoadMainMenu);
    }

    void LoadGame(string gameScene)
    {
        SceneManager.LoadScene(gameScene);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}