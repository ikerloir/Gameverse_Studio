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
        MortalBagIntro,     // 3_MortalBag (new intro scene)
        UltimateDefense,    // 4_UltimateDefense
        ZeroZoneVR          // 5_ZeroZoneVR
    }

    // Lista de juegos en orden para el modo Guerra Total
    private GameScenes[] warModeGames = new GameScenes[]
    {
        GameScenes.IntroAirboneDanger,
        GameScenes.UltimateDelivery,
        GameScenes.MortalBagIntro,
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
    //Singleton
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
        ScoreManager.Instance.ResetScores(); // Reinicia puntuaciones

        // musica de cada miniJuego
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
        

        // Si estamos en la escena "Intro", esperamos 3 segundos y cargamos "Menu"
        if (SceneManager.GetActiveScene().name == "Intro")
        {

            MusicManager.Instance.PlayMusic(MusicManager.Instance.introMusic, false);
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


    //carga de escenas y asigna valor a isWArMode segun sellecion de modo de juego
    public void ButtonLoadScene(GameScenes scene)
    {
        if (scene == GameScenes.Menu)
        {
            isWarMode = false;
            currentGameIndex = 0;
            Debug.Log($"GameManager: Reseteando índice a 0 al cargar Menu");
        }
        SceneManager.LoadScene(scene.ToString());
    }
    //modo WarMode  
    public void StartWarMode()
    {
        isWarMode = true;
        currentGameIndex = 0;
        Debug.Log($"GameManager: Iniciando modo guerra. Índice actual: {currentGameIndex}");
        LoadNextWarModeGame();
    }
    //carga de juegos y musica secuencialmente si esta en modo gerra si no vuelve.
    public void LoadNextWarModeGame()
    {
        if (!isWarMode) return;

        Debug.Log($"GameManager: Cargando siguiente juego. Índice actual: {currentGameIndex}");

        if (currentGameIndex < warModeGames.Length)
        {
            MusicManager.Instance.PlayMusic(warModeMusic[currentGameIndex], true);
            SceneManager.LoadScene(warModeGames[currentGameIndex].ToString());
            currentGameIndex++;
            Debug.Log($"GameManager: Índice incrementado a: {currentGameIndex}. Siguiente juego: {warModeGames[currentGameIndex - 1]}");
        }
        else
        {
            // Hemos completado todos los juegos
            isWarMode = false;
            currentGameIndex = 0;
            Debug.Log($"GameManager: Completado todos los juegos. Reseteando índice a 0");
            MusicManager.Instance.PlayMusic(MusicManager.Instance.scoreScene, true);
            SceneManager.LoadScene("Score"); // O la escena que prefieras para mostrar la puntuación final

        }
    }
    // carga Menu pasado unos segundos con efecto de sonida intro
    private IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Menu");        

        MusicManager.Instance.PlayMusic(MusicManager.Instance.menuMusic, true);
    }
    // conocer en que modo estamos jugando
    public bool IsInWarMode()
    {
        return isWarMode;
    }
}
