using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class GameManagerAirboneDanger : MonoBehaviour
{

    public bool isGameActive = true;
    public TextMeshProUGUI gameOver;
    public TextMeshProUGUI puntuacionTexto;
    public TextMeshProUGUI escudoTexto;
    private GameObject jugador;
    public Button siguienteJuego;
    public Button seguir;
    public AudioSource audioSourceGameOver;

    private int puntuacion = 0;
    private int escudo = 5;
    public int Escudo => escudo;
    public GameOverAirboneDanger gameOverAirboneDanger;


    public void StartGame()
    {
        
        UpdateScoreDisplay();        
        UpdateScoreEscudo();
        jugador = GameObject.FindGameObjectWithTag("Jugador");
        

    }
    // logica fin de juego
    public void GameOver()
    {
        isGameActive = false;
        gameOver.gameObject.SetActive(true);
        audioSourceGameOver.Play();
       
        siguienteJuego.gameObject.SetActive(true);
        seguir.gameObject.SetActive(false);

        ScoreManager.Instance.SetScore(0, puntuacion);
        gameOverAirboneDanger.MostrarResultado(puntuacion);




    }
    public void UpdateScore(int score)
    {
        puntuacion += score;  

        if (puntuacion <= 0)
        {
            puntuacion = 0;
        }
        UpdateScoreDisplay();
       
    }
    public void UpdateEscudo(int removeEsc)
    {
        escudo -= removeEsc;
        escudoTexto.text = "Escudo: " + escudo;
       

        if (escudo == 0)
        {
            Destroy(jugador);
            //acabar juego
            GameOver();
        }
    }
    
    private void UpdateScoreDisplay()
    {
        puntuacionTexto.text = "Score: " + puntuacion;
    }
    private void UpdateScoreEscudo()
    {
        escudoTexto.text = "Escudo: " + escudo;
    }

    
}