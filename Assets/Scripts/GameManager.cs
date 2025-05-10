using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int globalScore = 0;
    private bool isWarMode = false;
    private int currentGameIndex = 0;
    private AudioClip[] warModeMusic;



    // Enum con todas las escenas disponibles
    public enum GameScenes
    {
        Intro,
        Menu,
        GameSelect,
        Score,
        IntroAirboneDanger,      // 1_AirboneDanger
        UltimateDelivery,   // 2_UltimateDelivery
        MortalBag,          // 3_MortalBag
        UltimateDefense,    // 4_UltimateDefense
        ZeroZoneVR          // 5_ZeroZoneVR
    }

    // Lista de juegos en orden para el modo Guerra Total
    private GameScenes[] warModeGames = new GameScenes[]
    {
        GameScenes.IntroAirboneDanger,
        GameScenes.UltimateDelivery,
        GameScenes.MortalBag,
        GameScenes.UltimateDefense,
        GameScenes.ZeroZoneVR
    };

   


    [System.Serializable]
    public class SceneButtonPair
    {
        public GameScenes scene;
        public Button button;
    }

    public List<SceneButtonPair> sceneButtons;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (MusicManager.Instance != null)     
        {
            warModeMusic = new AudioClip[]
            {
            MusicManager.Instance.introAirboneDangerMusic,
            MusicManager.Instance.ultimateDeliveryMusic,
            MusicManager.Instance.mortalBagMusic,
            MusicManager.Instance.ultimateDefenseMusic,
            MusicManager.Instance.zeroZoneVRMusic
            };
        }
        else
        {
            Debug.LogError("MusicManager.Instance es NULL, asegúrate de que MusicManager esté en la escena inicial.");
        }

        // Si estamos en la escena "Intro", esperamos 3 segundos y cargamos "Menu"
        if (SceneManager.GetActiveScene().name == "Intro")
        {
           
            MusicManager.Instance.PlayMusic(MusicManager.Instance.introMusic,false);
            StartCoroutine(LoadMenuAfterDelay(3f));
        }
        

        // Asignar botones desde el Inspector
        foreach (var sceneButton in sceneButtons)
        {
            if (sceneButton.button != null)
            {
                sceneButton.button.onClick.AddListener(() => ButtonLoadScene(sceneButton.scene));
            }
        }
    }

    public void AddScore(int score)
    {
        globalScore += score;
    }

    public void ButtonLoadScene(GameScenes scene)
    {
        if (scene == GameScenes.Menu)
        {
            isWarMode = false;
            currentGameIndex = 0;
        }
        SceneManager.LoadScene(scene.ToString());
    }

    public void StartWarMode()
    {
        isWarMode = true;
        currentGameIndex = 0;
        LoadNextWarModeGame();
    }

    public void LoadNextWarModeGame()
    {
        if (!isWarMode) return;

        if (currentGameIndex < warModeGames.Length)
        {
            MusicManager.Instance.PlayMusic(warModeMusic[currentGameIndex],true);
            SceneManager.LoadScene(warModeGames[currentGameIndex].ToString());
            currentGameIndex++;
        }
        else
        {
            // Hemos completado todos los juegos
            isWarMode = false;
            currentGameIndex = 0;
            MusicManager.Instance.PlayMusic(MusicManager.Instance.scoreScene, true);
            SceneManager.LoadScene("Score"); // O la escena que prefieras para mostrar la puntuación final
            
        }
    }

    private IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Menu");

        //nuevo
        
        MusicManager.Instance.PlayMusic(MusicManager.Instance.menuMusic,true);
    }

    public bool IsInWarMode()
    {
        return isWarMode;
    }
}
