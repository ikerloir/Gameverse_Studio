using UnityEngine;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
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

    private int currentKills = 0;
    private int totalDisparos = 0;
    private int disparosAcertados = 0;
    private float dañoTotalRecibido = 0f;
    private float dañoTotalInfligido = 0f;

    private bool gameEnded = false;

    private void Start()
    {
        victoriaText.SetActive(false);
        gameOverText.SetActive(false);
        panelResultados.SetActive(false);

        UpdateKillsDisplay();
    }

    public void UpdatePlayerHealth(float current, float max)
    {
        if (healthText != null)
            healthText.text = $"{current:F0} / {max:F0}";

        if (current <= 0 && !gameEnded)
        {
            StartCoroutine(GameOverSequence());
        }
    }

    public void ShowDamageReceived(float damage)
    {
        dañoTotalRecibido += damage;

        if (damageReceivedText != null)
        {
            damageReceivedText.text = "-" + damage.ToString("F0");
            damageReceivedText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideDamageText));
            Invoke(nameof(HideDamageText), 1.5f);
        }
    }

    private void HideDamageText()
    {
        if (damageReceivedText != null)
            damageReceivedText.gameObject.SetActive(false);
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
        {
            StartCoroutine(VictoriaSequence());
        }
    }

    private void UpdateKillsDisplay()
    {
        if (killsText != null)
            killsText.text = currentKills.ToString();
    }

    private IEnumerator GameOverSequence()
    {
        gameEnded = true;
        gameOverText.SetActive(true);
        audioSource.PlayOneShot(sonidoGameOver);
        yield return new WaitForSeconds(2f);
        gameOverText.SetActive(false);
        MostrarResultados();
    }

    private IEnumerator VictoriaSequence()
    {
        gameEnded = true;
        victoriaText.SetActive(true);
        audioSource.PlayOneShot(sonidoVictoria);
        yield return new WaitForSeconds(2f);
        victoriaText.SetActive(false);
        MostrarResultados();
    }

    private void MostrarResultados()
    {
        panelResultados.SetActive(true);
        audioSource.PlayOneShot(sonidoResultados);

        float ratioAcierto = totalDisparos > 0 ? (float)disparosAcertados / totalDisparos : 0f;
        string valoracion = CalcularValoracion(ratioAcierto);

        resultadoTexto.text =
            $"Disparos: {totalDisparos}\n" +
            $"Aciertos: {disparosAcertados} ({(ratioAcierto * 100f):F1}%)\n" +
            $"Daño recibido: {dañoTotalRecibido:F1}\n" +
            $"Daño infligido: {dañoTotalInfligido:F1}\n" +
            $"Enemigos derrotados: {currentKills}\n" +
            $"Valoración: {valoracion}";
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
}
