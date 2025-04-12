using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooter : MonoBehaviour
{
    [Header("Prefabs de proyectil")]
    public GameObject bulletPrefab;
    public GameObject damageBulletPrefab;

    [Header("Puntos de disparo")]
    public Transform muzzle1;
    public Transform muzzle2;

    [Header("Ajustes de disparo")]
    public float bulletSpeed = 15f;
    public float fireRate = 0.3f;
    public bool isStarPowerActive = false;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("🖱️ [WeaponShooter] Click en editor - Disparo");
                Shoot();
            }
        }
        else
        {
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                Debug.Log("👆 [WeaponShooter] Toque en pantalla - Disparo");
                Shoot();
            }
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
        if (muzzle == null)
        {
            Debug.LogWarning("⚠️ [WeaponShooter] Muzzle no asignado.");
            return;
        }

        Vector3 spawnPosition = muzzle.position;
        Vector3 shootDirection = muzzle.forward;
        Quaternion spawnRotation = Quaternion.LookRotation(shootDirection);

        GameObject prefabToUse = (isStarPowerActive && damageBulletPrefab != null) ? damageBulletPrefab : bulletPrefab;
        GameObject bullet = Instantiate(prefabToUse, spawnPosition, spawnRotation);

        Debug.Log($"🚀 [WeaponShooter] Bala creada en {spawnPosition} hacia {shootDirection}");

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = shootDirection * bulletSpeed;
            Debug.Log("[WeaponShooter] Velocidad aplicada a la bala.");
        }

        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.targetTag = "Enemy";
        }

        Collider bulletCol = bullet.GetComponent<Collider>();
        Collider ownCol = GetComponent<Collider>();
        if (bulletCol != null && ownCol != null)
        {
            Physics.IgnoreCollision(bulletCol, ownCol);
        }

        bullet.transform.localScale = Vector3.one * 0.1f;
        Debug.DrawRay(spawnPosition, shootDirection * 2f, Color.red, 2f);

        Destroy(bullet, 5f);
    }
}
