using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class NextGameButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
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
                GameManager.Instance.LoadScene(GameManager.GameScenes.Menu);
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