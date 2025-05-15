using UnityEngine;

public static class DamageEffects
{
    public static void ShowFloatingText(GameObject prefab, Vector3 position, float amount)
    {
        if (prefab == null) return;

        var obj = GameObject.Instantiate(prefab, position + Vector3.up * 0.5f, Quaternion.identity);
        obj.GetComponent<FloatingDamageText>()?.SetDamage(amount);
    }

    public static void TriggerPlayerFlash(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            var flash = GameObject.FindFirstObjectByType<ScreenDamageFlash>();
            flash?.TriggerFlash();
        }
    }

    public static void ShakeCamera(CameraShake shake)
    {
        if (shake != null)
            shake.TriggerShake();
    }

    public static void PlaySound(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
            source.PlayOneShot(clip);
    }
}
