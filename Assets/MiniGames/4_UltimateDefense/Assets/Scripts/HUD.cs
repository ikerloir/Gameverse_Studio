using UnityEngine;
using UnityEngine.UI;

public class LifeHUD : MonoBehaviour
{
    public Slider lifeSlider;

    public void UpdateLife(int value)
    {
        if (lifeSlider != null)
            lifeSlider.value = value;
    }
}
