using UnityEngine;
using TMPro;

public class LifeHUD : MonoBehaviour
{
    public TextMeshProUGUI lifeText;

    public void UpdateLife(int value)
    {
        if (lifeText != null)
            lifeText.text = "" + value.ToString();
    }
}
