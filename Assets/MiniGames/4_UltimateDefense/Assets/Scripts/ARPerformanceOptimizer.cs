using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPerformanceOptimizer : MonoBehaviour
{
    [Header("FPS Control")]
    public float fpsCheckInterval = 2f;
    public int lowFpsThreshold = 25;
    public int targetFrameRate = 60;

    [Header("Quality Settings")]
    public int lowQualityLevel = 0;
    public int highQualityLevel = 2;

    private float fpsTimer;
    private int frames;
    private float currentFps;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.SetQualityLevel(highQualityLevel);
        fpsTimer = 0;
        frames = 0;

        // Opcional: Desactivar auto focus si no es necesario
        var cameraManager = FindObjectOfType<ARCameraManager>();
        if (cameraManager != null)
        {
            cameraManager.autoFocusRequested = false;
        }
    }

    void Update()
    {
        frames++;
        fpsTimer += Time.unscaledDeltaTime;

        if (fpsTimer >= fpsCheckInterval)
        {
            currentFps = frames / fpsTimer;

            if (currentFps < lowFpsThreshold)
            {
                Debug.LogWarning($"Low FPS detected: {currentFps:F1}. Lowering quality.");
                QualitySettings.SetQualityLevel(lowQualityLevel);
            }

            frames = 0;
            fpsTimer = 0f;
        }
    }
}
