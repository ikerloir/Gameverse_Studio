using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle1;
    public Transform muzzle2;
    public float bulletSpeed = 15f;
    public float fireRate = 0.3f;

    public AudioSource shootAudioSource; // Añadido: Referencia al AudioSource para sonido de disparo

    private float nextFireTime = 0f;
    private bool useFirstMuzzle = true; // Alternar cañones

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

            if (useFirstMuzzle)
                ShootFromMuzzle(muzzle1);
            else
                ShootFromMuzzle(muzzle2);

            useFirstMuzzle = !useFirstMuzzle;
        }
    }

    void ShootFromMuzzle(Transform muzzle)
    {
        if (muzzle == null) return;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);

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

        var hud = FindFirstObjectByType<HUDManager>();
        if (hud != null)
        {
            hud.RegisterProjectileFired();
        }

        // Añadido: Reproducir sonido de disparo
        if (shootAudioSource != null)
        {
            shootAudioSource.Play();
        }

        Destroy(bullet, 5f);
    }
}
