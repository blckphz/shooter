using UnityEngine;
using System.Collections;

public class gunShooting : MonoBehaviour
{
    private Coroutine autoShootCoroutine;
    private Coroutine burstShootCoroutine;
    private bool isBurstShooting = false;

    private GunReload gunReload;
    private GunBehaviour gunBehaviour;

    void Start()
    {
        gunReload = GetComponent<GunReload>();
        gunBehaviour = GetComponent<GunBehaviour>();
    }

    public void HandleShootingInput(GunSO currentGunso)
    {
        if (currentGunso == null) return;

        switch (currentGunso.fireMode)
        {
            case FireMode.SemiAuto:
                if (Input.GetButtonDown("Fire1") && !currentGunso.IsReloading)
                {
                    TryShoot(currentGunso);
                }
                break;

            case FireMode.Auto:
                if (Input.GetButtonDown("Fire1") && !currentGunso.IsReloading && autoShootCoroutine == null)
                {
                    isBurstShooting = true;
                    autoShootCoroutine = StartCoroutine(AutoShoot(currentGunso));
                }
                break;

            case FireMode.Burst:
                if (Input.GetButtonDown("Fire1") && !isBurstShooting && burstShootCoroutine == null && !currentGunso.IsReloading && currentGunso.currentClipSize > 0)
                {
                    isBurstShooting = true;
                    burstShootCoroutine = StartCoroutine(BurstShoot(currentGunso));
                }
                break;
        }
    }

    void TryShoot(GunSO currentGunso)
    {
        if (currentGunso == null) return;

        if (currentGunso.IsReloading)
        {
            Debug.Log("Cannot shoot, reloading in progress...");
            return;
        }

        if (currentGunso.cooldownTimer > 0f) return;

        if (currentGunso.currentClipSize <= 0)
        {
            gunReload.StartReload(currentGunso);
            Debug.Log("Clip empty! Reloading...");
            return;
        }

        if (!(currentGunso is Grappling))
        {
            SpawnBullet(currentGunso);
        }

        currentGunso.currentClipSize--;

        if (currentGunso.fireMode == FireMode.SemiAuto)
        {
            currentGunso.cooldownTimer = currentGunso.shootCooldown;
        }

        currentGunso.ShootGun(gunBehaviour.spawnPoint, currentGunso.bulletSpeed);

        if (currentGunso.currentClipSize <= 0 && !currentGunso.IsReloading)
        {
            gunReload.StartReload(currentGunso);
            Debug.Log("Clip empty! Reloading...");
        }
    }

    IEnumerator BurstShoot(GunSO currentGunso)
    {
        int shotsFired = 0;

        while (shotsFired < currentGunso.burstCount && currentGunso.currentClipSize > 0)
        {
            while (currentGunso.cooldownTimer > 0f)
                yield return null;

            TryShoot(currentGunso);

            shotsFired++;
            currentGunso.cooldownTimer = 1f / currentGunso.ShootFreq;
            yield return null;
        }

        currentGunso.cooldownTimer = currentGunso.shootCooldown;
        isBurstShooting = false;
        burstShootCoroutine = null;
    }

    IEnumerator AutoShoot(GunSO currentGunso)
    {
        while (Input.GetButton("Fire1") && currentGunso.currentClipSize > 0 && !currentGunso.IsReloading)
        {
            if (currentGunso.cooldownTimer <= 0f)
            {
                TryShoot(currentGunso);
                currentGunso.cooldownTimer = 1f / currentGunso.ShootFreq;
            }
            yield return null;
        }

        currentGunso.cooldownTimer = currentGunso.shootCooldown;
        isBurstShooting = false;
        autoShootCoroutine = null;
    }

    void SpawnBullet(GunSO currentGunso)
    {
        GameObject bullet = Instantiate(currentGunso.BulletPrefab, gunBehaviour.spawnPoint.position, gunBehaviour.spawnPoint.rotation);
        bullet.transform.Rotate(0f, 0f, 90f);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = gunBehaviour.spawnPoint.forward * currentGunso.bulletSpeed;
        }

        Destroy(bullet, 5f);
    }

    public void StopShootingCoroutines()
    {
        if (autoShootCoroutine != null)
        {
            StopCoroutine(autoShootCoroutine);
            autoShootCoroutine = null;
        }

        if (burstShootCoroutine != null)
        {
            StopCoroutine(burstShootCoroutine);
            burstShootCoroutine = null;
        }

        isBurstShooting = false;
    }
}
