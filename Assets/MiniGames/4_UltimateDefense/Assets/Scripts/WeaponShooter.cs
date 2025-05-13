using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle1;
    public Transform muzzle2;

    public float bulletSpeed = 15f;
    public float fireRate = 0.3f;

    private float nextFireTime = 0f;

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

            ShootFromMuzzle(muzzle1);
            ShootFromMuzzle(muzzle2);
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

        Destroy(bullet, 5f);
    }
}
