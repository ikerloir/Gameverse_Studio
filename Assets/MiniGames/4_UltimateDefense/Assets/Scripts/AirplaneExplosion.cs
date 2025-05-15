using UnityEngine;

public class AirplaneExplosion : MonoBehaviour
{
    [Header("Configuración Explosión")]
    public Gradient colorGradient;
    public AnimationCurve sizeCurve;
    public ParticleSystem explosionParticleSystem;

    void Awake()
    {
        // Crear el Particle System en tiempo de ejecución de forma segura
        if (explosionParticleSystem == null)
        {
            explosionParticleSystem = gameObject.AddComponent<ParticleSystem>();
            ConfigureParticleSystem(explosionParticleSystem);
        }
    }

    void Start()
    {
        explosionParticleSystem.Play();
    }

    void ConfigureParticleSystem(ParticleSystem ps)
    {
        var main = ps.main;
        main.duration = 2.0f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(1.0f, 2.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(10f, 30f);
        main.startSize = new ParticleSystem.MinMaxCurve(3f, 6f);
        main.startColor = new ParticleSystem.MinMaxGradient(Color.yellow, Color.red);
        main.gravityModifier = 0.3f;
        main.loop = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 500;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.burstCount = 1;
        emission.SetBurst(0, new ParticleSystem.Burst(0.0f, 200, 300));

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 2.0f;

        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        if (colorGradient == null || colorGradient.colorKeys.Length == 0)
        {
            colorGradient = new Gradient();
            colorGradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.yellow, 0.0f),
                    new GradientColorKey(Color.red, 0.5f),
                    new GradientColorKey(new Color(0.1f, 0.1f, 0.1f), 1.0f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1.0f, 0.0f),
                    new GradientAlphaKey(0.5f, 0.5f),
                    new GradientAlphaKey(0.0f, 1.0f)
                }
            );
        }
        colorOverLifetime.color = colorGradient;

        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        if (sizeCurve == null || sizeCurve.length == 0)
        {
            sizeCurve = new AnimationCurve();
            sizeCurve.AddKey(0.0f, 1.0f);
            sizeCurve.AddKey(0.5f, 1.5f);
            sizeCurve.AddKey(1.0f, 0.0f);
        }
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, sizeCurve);

        var lights = ps.lights;
        lights.enabled = true;
        lights.ratio = 0.1f;
        lights.intensityMultiplier = 5.0f;
        lights.rangeMultiplier = 10.0f;

        var noise = ps.noise;
        noise.enabled = true;
        noise.strength = 2.0f;
        noise.frequency = 0.5f;
        noise.scrollSpeed = 0.2f;
    }
}
