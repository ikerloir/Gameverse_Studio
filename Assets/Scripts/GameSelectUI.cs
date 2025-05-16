using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSelectUI : MonoBehaviour
{
    //botones de los juegos
    public Button game1Button;
    public Button game2Button;
    public Button game3Button;
    public Button gameARButton;
    public Button gameVRButton;
    public Button backButton;
    private MusicManager musicManager;

    void Start()
    {
        musicManager = MusicManager.Instance;

        game1Button.onClick.AddListener(() => LoadGame("IntroAirboneDanger"));
        game2Button.onClick.AddListener(() => LoadGame("UltimateDelivery"));
        game3Button.onClick.AddListener(() => LoadGame("MortalBagIntro"));
        gameARButton.onClick.AddListener(() => LoadGame("UltimateDefense"));
        gameVRButton.onClick.AddListener(() => LoadGame("ZeroZoneVR"));
        backButton.onClick.AddListener(LoadMainMenu);
    }
    // carga de música segun juego elegido y la Scena
    void LoadGame(string gameScene)
    {
        if (musicManager != null)
        {
            switch (gameScene)
            {
                case "IntroAirboneDanger":
                    musicManager.PlayMusic(musicManager.introAirboneDangerMusic, true);
                    break;
                case "UltimateDelivery":
                    musicManager.PlayMusic(musicManager.ultimateDeliveryMusic, true);
                    break;
                case "MortalBag":
                    musicManager.PlayMusic(musicManager.mortalBagMusic, true);
                    break;
                case "UltimateDefense":
                    musicManager.PlayMusic(musicManager.ultimateDefenseMusic, true);
                    break;
                case "ZeroZoneVR":
                    musicManager.PlayMusic(musicManager.zeroZoneVRMusic, true);
                    break;
                default:
                   
                    break;
            }
        }
        
        SceneManager.LoadScene(gameScene);
    }
    // carga Scena Menu
    void LoadMainMenu()
    {
        
        SceneManager.LoadScene("Menu");
    }
}