using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int globalScore = 0;

    // Enum con todas las escenas disponibles
    public enum GameScenes
    {
        Intro,
        Menu,
        GameSelect,
        Score,
        Game2D,
        Game3DA,
        Game3DB,
        GameAR,
        GameVR
    }

    [System.Serializable]
    public class SceneButtonPair
    {
        public GameScenes scene; // Escena a la que cambiará el botón
        public Button button;    // Botón que activará la escena
    }

    public List<SceneButtonPair> sceneButtons; // Lista de botones asignables en el Inspector

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
        // Si estamos en la escena "Intro", esperamos 3 segundos y cargamos "Menu"
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            StartCoroutine(LoadMenuAfterDelay(3f));
        }

        // Asignar botones desde el Inspector
        foreach (var sceneButton in sceneButtons)
        {
            if (sceneButton.button != null)
            {
                sceneButton.button.onClick.AddListener(() => LoadScene(sceneButton.scene));
            }
        }
    }

    public void AddScore(int score)
    {
        globalScore += score;
    }

    public void LoadScene(GameScenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    private IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Menu");
    }
}
