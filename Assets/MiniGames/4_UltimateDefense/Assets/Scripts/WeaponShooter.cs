using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    public string projectileTag = "Projectile";
    public Transform muzzle1;
    public Transform muzzle2;
    public float bulletSpeed = 15f;
    public float fireRate = 0.3f;

    public AudioSource shootAudioSource;

    private float nextFireTime = 0f;
    private bool useFirstMuzzle = true;

    private static HUDManager hud;

    void Start()
    {
        if (hud == null)
        {
            hud = FindObjectOfType<HUDManager>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Transform selectedMuzzle = useFirstMuzzle ? muzzle1 : muzzle2;
            useFirstMuzzle = !useFirstMuzzle;

            ShootFromMuzzle(selectedMuzzle);
        }
    }

    void ShootFromMuzzle(Transform muzzle)
    {
        if (muzzle == null || ObjectPooler.Instance == null) return;

        GameObject bullet = ObjectPooler.Instance.SpawnFromPool(projectileTag, muzzle.position, muzzle.rotation);
        if (bullet.TryGetComponent(out Projectile proj))
        {
            proj.targetTag = "Enemy";
            proj.SetDirection(muzzle.forward);
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = muzzle.forward * bulletSpeed;
        }

        hud?.RegisterProjectileFired();

        if (shootAudioSource != null)
        {
            shootAudioSource.Play();
        }
    }
}
