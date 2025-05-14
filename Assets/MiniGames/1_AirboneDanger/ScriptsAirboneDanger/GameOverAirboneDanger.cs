using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverAirboneDanger : MonoBehaviour
{
    
    
    public TextMeshProUGUI medalText;
    public Image medalImage;
    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;
    public Sprite doubleGold;
    public Sprite tripleGold;

   
    

    public void MostrarResultado(int score)
    {
        medalImage.gameObject.SetActive(true);
        medalText.gameObject.SetActive(true);

        if (score < 20)
            medalImage.sprite = bronze;
        else if (score >= 20 && score < 40)
            medalImage.sprite = silver;
        else if (score >= 40 && score < 60)
            medalImage.sprite = gold;
        else if (score >= 60 && score < 80)
            medalImage.sprite = doubleGold;
        else
            medalImage.sprite = tripleGold;
        Debug.Log(score);
    }
    
}



