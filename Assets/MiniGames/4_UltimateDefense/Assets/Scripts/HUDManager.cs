using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Referencias de Texto")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageReceivedText;
    public TextMeshProUGUI killsText;

    private int currentKills = 0;
    private Vector3 originalDamageTextPosition;
    private Color originalDamageTextColor;
    private float fadeDuration = 1.5f;
    private float fadeTimer;

    void Start()
    {
        if (damageReceivedText != null)
        {
            originalDamageTextPosition = damageReceivedText.rectTransform.localPosition;
            originalDamageTextColor = damageReceivedText.color;
            damageReceivedText.gameObject.SetActive(false);
        }
    }

    public void UpdateHealth(int value)
    {
        if (healthText != null)
            healthText.text = "" + value.ToString();
    }

    public void ShowDamageReceived(float damage)
    {
        if (damageReceivedText != null)
        {
            damageReceivedText.text = "-" + damage.ToString("F0");
            damageReceivedText.rectTransform.localPosition = originalDamageTextPosition;
            damageReceivedText.color = originalDamageTextColor;
            damageReceivedText.gameObject.SetActive(true);
            fadeTimer = fadeDuration;

            CancelInvoke(nameof(HideDamageText));
            CancelInvoke(nameof(AnimateDamageText));
            InvokeRepeating(nameof(AnimateDamageText), 0f, 0.016f);
            Invoke(nameof(HideDamageText), fadeDuration);
        }
    }

    private void AnimateDamageText()
    {
        if (damageReceivedText != null)
        {
            damageReceivedText.rectTransform.localPosition += new Vector3(0, 0.5f, 0);
            fadeTimer -= 0.016f;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            Color color = damageReceivedText.color;
            color.a = alpha;
            damageReceivedText.color = color;
        }
    }

    private void HideDamageText()
    {
        if (damageReceivedText != null)
        {
            CancelInvoke(nameof(AnimateDamageText));
            damageReceivedText.gameObject.SetActive(false);
            damageReceivedText.rectTransform.localPosition = originalDamageTextPosition;
            damageReceivedText.color = originalDamageTextColor;
        }
    }

    public void AddKill()
    {
        currentKills++;
        if (killsText != null)
            killsText.text = "" + currentKills.ToString();
    }
}