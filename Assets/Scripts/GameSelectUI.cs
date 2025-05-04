using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSelectUI : MonoBehaviour
{
    public Button game1Button;
    public Button game2Button;
    public Button game3Button;
    public Button gameARButton;
    public Button gameVRButton;
    public Button backButton;

    void Start()
    {
        game1Button.onClick.AddListener(() => LoadGame("IntroAirboneDanger"));
        game2Button.onClick.AddListener(() => LoadGame("UltimateDelivery"));
        game3Button.onClick.AddListener(() => LoadGame("MortalBag"));
        gameARButton.onClick.AddListener(() => LoadGame("UltimateDefense"));
        gameVRButton.onClick.AddListener(() => LoadGame("ZeroZoneVR"));
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