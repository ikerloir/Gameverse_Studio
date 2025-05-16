using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreScene : MonoBehaviour
{
    public TextMeshProUGUI puntuacionTexto; // puntuación total
    public Sprite goldenMedal;
    public Sprite silverMedal;
    public Sprite bronzeMedal;
    public Image finalMedal;
    public Image finalMedalDoubleGold; 
    public Image finalMedalTripleGold;

    void Start()
    {
        // puntuación total utilizando el ScoreManager
        float rating = ScoreManager.Instance.CalculateFinalStarRating();

        // Llama al método para mostrar las estrellas
        MostrarMedallaFinal(rating);
    }
    //Muestra las medallas segun el rating obtenido de 1 a 5
    void MostrarMedallaFinal(float rating)
    {
        finalMedalDoubleGold.gameObject.SetActive(false);
        finalMedalTripleGold.gameObject.SetActive(false);

        if (rating < 1.0f)
        {
            finalMedal.sprite = bronzeMedal;
        }
        else if (rating < 2.0f)
        {
            finalMedal.sprite = silverMedal;
        }
        else if (rating < 3.0f)
        {
            finalMedal.sprite = goldenMedal;
        }
        else if (rating < 4.0f)
        {
            //doble oro
            finalMedal.sprite = goldenMedal;
            finalMedalDoubleGold.sprite = goldenMedal;
            finalMedalDoubleGold.gameObject.SetActive(true);

        }
        else
        {
            // sprite para triple oro
            finalMedal.sprite = goldenMedal;
            finalMedalDoubleGold.sprite = goldenMedal;
            finalMedalDoubleGold.gameObject.SetActive(true);
            finalMedalTripleGold.sprite = goldenMedal;
            finalMedalTripleGold.gameObject.SetActive(true);
        }
        
    }
}