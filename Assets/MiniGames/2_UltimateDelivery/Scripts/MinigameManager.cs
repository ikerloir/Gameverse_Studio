
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Canvas introCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas outroCanvas;

    [SerializeField] private GameObject player;

    void Start()
    {
        introCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        outroCanvas.gameObject.SetActive(false);

        if (player != null)
        {
            player.SetActive(false);
        }
    }

    public void StartMinigame()
    {
        introCanvas.gameObject.SetActive(false);
        outroCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);

        if (player != null)
        {
            player.SetActive(true);
        }
    }

    public void ReturnToMainGame()
    {
        // Connect to the "Back" button in the main game.        
    }

    public void ResetMinigame()
    {
        introCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
        outroCanvas.gameObject.SetActive(false);

        if (player != null)
        {
            player.SetActive(false);
        }
    }
}