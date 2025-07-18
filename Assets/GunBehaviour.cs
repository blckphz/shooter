using UnityEngine;
using System.Collections;

public class GunBehaviour : MonoBehaviour
{
    public GunSO gunso;
    public Transform spawnPoint;
    public float bulletSpeed = 20f;

    private bool isReloading = false;
    private bool isBurstShooting = false;

    void Start()
    {
        gunso.currentClipSize = gunso.maxClipSize;
        gunso.cooldownTimer = 0f;
    }

    void Update()
    {
        if (gunso.cooldownTimer > 0f)
            gunso.cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && gunso.currentClipSize < gunso.maxClipSize)
        {
            StartCoroutine(Reload());
        }

        switch (gunso.fireMode)
        {
            case FireMode.SemiAuto:
                if (Input.GetButtonDown("Fire1"))
                {
                    TryShoot();
                }
                break;

            case FireMode.Auto:
                if (Input.GetButton("Fire1"))
                {
                    TryShoot();
                }
                break;

            case FireMode.Burst:
                if (Input.GetButtonDown("Fire1") && !isBurstShooting)
                {
                    StartCoroutine(BurstShoot());
                }
                break;
        }
    }

    void TryShoot()
    {
        if (gunso.cooldownTimer > 0f) return;
        if (gunso.currentClipSize <= 0)
        {
            if (!isReloading)
            {
                StartCoroutine(Reload()); // Auto reload when empty
                Debug.Log("Clip empty! Reloading...");
            }
            return;
        }

        if (!(gunso is Grappling)) // Only spawn normal bullets
        {
            SpawnBullet();
        }

        gunso.currentClipSize--;
        gunso.cooldownTimer = gunso.shootCooldown;

        gunso.ShootGun(spawnPoint, bulletSpeed);

        // Auto reload if we just fired the last bullet
        if (gunso.currentClipSize <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
            Debug.Log("Clip empty! Reloading...");
        }
    }



    IEnumerator BurstShoot()
    {
        isBurstShooting = true;

        int shotsFired = 0;
        while (shotsFired < gunso.burstCount && gunso.currentClipSize > 0)
        {
            if (gunso.cooldownTimer <= 0f)
            {
                TryShoot();
                shotsFired++;
            }
            else
            {
                yield return null; // wait until cooldown finishes
            }
        }

        isBurstShooting = false;
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(gunso.BulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.transform.Rotate(0f, 0f, 90f);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Use full forward direction of the gun for realistic aiming
            rb.linearVelocity = spawnPoint.forward * bulletSpeed;
        }


        Destroy(bullet, 5f);
    }


    IEnumerator Reload()
    {
        isReloading = true;
    
        yield return new WaitForSeconds(gunso.reloadTime);
        gunso.currentClipSize = gunso.maxClipSize;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
