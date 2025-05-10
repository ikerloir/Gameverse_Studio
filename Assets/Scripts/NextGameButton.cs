using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class NextGameButton : MonoBehaviour
{
    private Button button;
    private MusicManager musicManager;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        musicManager = MusicManager.Instance;
    }

    void OnButtonClick()
    {
        if (GameManager.Instance != null)
        {
            // Si estamos en modo guerra (loop), cargamos el siguiente juego
            if (GameManager.Instance.IsInWarMode())
            {
                GameManager.Instance.LoadNextWarModeGame();
            }
            // Si estamos en modo individual, volvemos al menú principal
            else
            {
                GameManager.Instance.ButtonLoadScene(GameManager.GameScenes.Menu);
                musicManager.PlayMusic(musicManager.menuMusic, true);
            }
        }
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}