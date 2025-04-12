using UnityEngine;

public class ScreenDamageFlash : MonoBehaviour
{
    public CanvasGroup flashGroup;
    public float flashDuration = 0.3f;
    public float maxAlpha = 0.4f;

    private float timer = 0f;

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            flashGroup.alpha = Mathf.Lerp(0f, maxAlpha, timer / flashDuration);
            if (timer <= 0f)
            {
                flashGroup.alpha = 0f;
                flashGroup.gameObject.SetActive(false);
            }
        }
    }

    public void TriggerFlash()
    {
        flashGroup.alpha = maxAlpha;
        flashGroup.gameObject.SetActive(true);
        timer = flashDuration;
    }
}
