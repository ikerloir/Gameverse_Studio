using UnityEngine;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI damageReceivedText;
    [SerializeField] private TextMeshProUGUI resultadoTexto;
    [SerializeField] private GameObject victoriaText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject panelResultados;

    [Header("Audios")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoVictoria;
    [SerializeField] private AudioClip sonidoGameOver;
    [SerializeField] private AudioClip sonidoResultados;

    [Header("Configuración")]
    [SerializeField] private int killsParaVictoria = 5;

    private const int myGameIndex = 3;

    private int currentKills = 0;
    private int totalDisparos = 0;
    private int disparosAcertados = 0;
    private float dañoTotalRecibido = 0f;
    private float dañoTotalInfligido = 0f;

    private bool gameEnded = false;
    private float playerCurrentHealth = 100f;
    private float playerMaxHealth = 100f;

    private void Awake()
    {
        // Singleton pattern para acceso global sin FindObjectOfType
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        victoriaText?.SetActive(false);
        gameOverText?.SetActive(false);
        panelResultados?.SetActive(false);

        UpdateKillsDisplay();
        UpdatePlayerHealth(playerCurrentHealth, playerMaxHealth);
    }

    public void UpdatePlayerHealth(float current, float max)
    {
        playerCurrentHealth = Mathf.Clamp(current, 0f, max);
        playerMaxHealth = max;

        if (healthText != null)
            healthText.text = $"{playerCurrentHealth:F0} / {playerMaxHealth:F0}";

        if (playerCurrentHealth <= 0 && !gameEnded)
            StartCoroutine(GameOverSequence());
    }

    public void AddLife(float amount)
    {
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth + amount, 0f, playerMaxHealth);
        UpdatePlayerHealth(playerCurrentHealth, playerMaxHealth);
    }

    public void ShowDamageReceived(float damage)
    {
        dañoTotalRecibido += damage;

        if (damageReceivedText != null)
        {
            damageReceivedText.text = $"-{damage:F0}";
            damageReceivedText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideDamageText));
            Invoke(nameof(HideDamageText), 1.5f);
        }
    }

    private void HideDamageText()
    {
        damageReceivedText?.gameObject.SetActive(false);
    }

    public void RegisterProjectileFired()
    {
        totalDisparos++;
    }

    public void AddShot(bool acertado)
    {
        if (acertado) disparosAcertados++;
    }

    public void AddDamageDealt(float amount)
    {
        dañoTotalInfligido += amount;
    }

    public void AddKill(float dañoInfligido)
    {
        currentKills++;
        dañoTotalInfligido += dañoInfligido;
        UpdateKillsDisplay();

        if (currentKills >= killsParaVictoria && !gameEnded)
            StartCoroutine(VictoriaSequence());
    }

    private void UpdateKillsDisplay()
    {
        if (killsText != null)
            killsText.text = currentKills.ToString();
    }

    private IEnumerator GameOverSequence()
    {
        gameEnded = true;
        gameOverText?.SetActive(true);
        PlaySound(sonidoGameOver);
        yield return new WaitForSeconds(2f);
        gameOverText?.SetActive(false);
        ReportarPuntuacionFinal(false);
        MostrarResultados();
    }

    private IEnumerator VictoriaSequence()
    {
        gameEnded = true;
        victoriaText?.SetActive(true);
        PlaySound(sonidoVictoria);
        yield return new WaitForSeconds(2f);
        victoriaText?.SetActive(false);
        ReportarPuntuacionFinal(true);
        MostrarResultados();
    }

    private void MostrarResultados()
    {
        panelResultados?.SetActive(true);
        PlaySound(sonidoResultados);

        float ratioAcierto = totalDisparos > 0 ? (float)disparosAcertados / totalDisparos : 0f;
        string valoracion = CalcularValoracion(ratioAcierto);

        if (resultadoTexto != null)
        {
            resultadoTexto.text =
                $"Disparos: {totalDisparos}\n" +
                $"Aciertos: {disparosAcertados} ({(ratioAcierto * 100f):F1}%)\n" +
                $"Daño recibido: {dañoTotalRecibido:F1}\n" +
                $"Daño infligido: {dañoTotalInfligido:F1}\n" +
                $"Enemigos derrotados: {currentKills}\n" +
                $"Valoración: {valoracion}";
        }

        StartCoroutine(AutoNextGameAfterDelay());
    }

    private string CalcularValoracion(float ratioAcierto)
    {
        float score = (ratioAcierto * 100f) + dañoTotalInfligido - dañoTotalRecibido + (currentKills * 10f);
        if (score > 200) return "S";
        if (score > 150) return "A";
        if (score > 100) return "B";
        if (score > 50) return "C";
        return "D";
    }

    private int ValoracionALevelScore(string valoracion)
    {
        switch (valoracion)
        {
            case "S": return 100;
            case "A": return 80;
            case "B": return 60;
            case "C": return 40;
            default: return 20;
        }
    }

    public void ReportarPuntuacionFinal(bool victoria)
    {
        int scoreFinal = 0;

        if (victoria)
        {
            float ratioAcierto = totalDisparos > 0 ? (float)disparosAcertados / totalDisparos : 0f;
            string valoracion = CalcularValoracion(ratioAcierto);
            scoreFinal = ValoracionALevelScore(valoracion);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScore(myGameIndex, scoreFinal);
            Debug.Log($"Puntuación reportada al ScoreManager en índice {myGameIndex}: {scoreFinal}");
        }
        else
        {
            Debug.LogWarning("ScoreManager.Instance no encontrado al reportar puntuación.");
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator AutoNextGameAfterDelay()
    {
        yield return new WaitForSeconds(4f);

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.IsInWarMode())
            {
                GameManager.Instance.LoadNextWarModeGame();
            }
            else
            {
                GameManager.Instance.ButtonLoadScene(GameManager.GameScenes.Menu);

                if (MusicManager.Instance != null)
                    MusicManager.Instance.PlayMusic(MusicManager.Instance.menuMusic, true);
            }
        }
    }
}
