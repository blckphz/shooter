using UnityEngine;
using System.Collections;

public class GunBehaviour : MonoBehaviour
{
    public GunSO[] guns;
    private int currentGunIndex = 0;

    public GunSO currentGunso;
    public Transform spawnPoint;

    [Header("Scale Settings")]
    public float gunXScale = 1f;
    public float gun2XScale = 2f;
    public float gun3YScale = 1.5f;

    private bool isReloading = false;
    private bool isBurstShooting = false;

    private Coroutine autoShootCoroutine;
    private Coroutine burstShootCoroutine;

    public bool IsReloading => isReloading;

    void Start()
    {
        if (guns.Length > 0)
        {
            currentGunIndex = 0;
            SwitchGun(guns[currentGunIndex]);
        }
        else
        {
            Debug.LogWarning("No guns assigned to the guns array!");
        }
    }

    void Update()
    {
        HandleGunSwitch();

        if (currentGunso == null) return;

        // Tick down cooldown
        if (currentGunso.cooldownTimer > 0f)
            currentGunso.cooldownTimer -= Time.deltaTime;

        // Manual reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentGunso.currentClipSize < currentGunso.maxClipSize)
        {
            StartCoroutine(Reload());
        }

        // Stop shooting when fire button is released
        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            StopShootingCoroutines();
        }

        // Special case for PortalGun
        if (currentGunso is PortalGun portalGun)
        {
            if (Input.GetButtonDown("Fire1") && !isReloading)
            {
                portalGun.ShootBluePortal(spawnPoint, currentGunso.bulletSpeed);
            }
            else if (Input.GetButtonDown("Fire2") && !isReloading)
            {
                portalGun.ShootOrangePortal(spawnPoint, currentGunso.bulletSpeed);
            }
            return; // Skip normal shooting logic
        }

        // Handle other guns
        switch (currentGunso.fireMode)
        {
            case FireMode.SemiAuto:
                if (Input.GetButtonDown("Fire1") && !isReloading)
                {
                    TryShoot();
                }
                break;

            case FireMode.Auto:
                if (Input.GetButton("Fire1") && !isReloading && autoShootCoroutine == null)
                {
                    autoShootCoroutine = StartCoroutine(AutoShoot());
                }
                break;

            case FireMode.Burst:
                if (Input.GetButton("Fire1") && !isBurstShooting && burstShootCoroutine == null)
                {
                    burstShootCoroutine = StartCoroutine(BurstShoot());
                }
                break;
        }
    }


    private void HandleGunSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (guns.Length == 0) return;
            currentGunIndex = (currentGunIndex + 1) % guns.Length;
            SwitchGun(guns[currentGunIndex]);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (guns.Length == 0) return;
            currentGunIndex--;
            if (currentGunIndex < 0)
                currentGunIndex = guns.Length - 1;
            SwitchGun(guns[currentGunIndex]);
        }
    }

    private void SwitchGun(GunSO newGun)
    {
        if (newGun == null) return;

        StopAllCoroutines(); // cancel ongoing actions like reload/shoot
        isReloading = false;
        isBurstShooting = false;
        autoShootCoroutine = null;
        burstShootCoroutine = null;

        currentGunso = newGun;
        InitializeGun(currentGunso);
        Debug.Log($"Switched to gun: {currentGunso.name}");

        // Adjust scale
        Vector3 scale = transform.localScale;
        scale.x = gunXScale;
        scale.y = gunXScale;
        if (currentGunIndex == 1)
        {
            scale.x = gun2XScale;
        }
        else if (currentGunIndex == 2)
        {
            scale.y = gun3YScale;
        }
        transform.localScale = scale;
    }

    private void InitializeGun(GunSO gun)
    {
        gun.cooldownTimer = 0f;
        if (gun.currentClipSize == 0)
        {
            gun.currentClipSize = gun.maxClipSize;
        }
    }

    void TryShoot()
    {
        if (currentGunso.cooldownTimer > 0f) return;

        if (currentGunso.currentClipSize <= 0)
        {
            if (!isReloading)
            {
                StartCoroutine(Reload());
                Debug.Log("Clip empty! Reloading...");
            }
            return;
        }

        if (!(currentGunso is Grappling))
        {
            SpawnBullet();
        }

        currentGunso.currentClipSize--;
        currentGunso.cooldownTimer = 1f / currentGunso.ShootFreq;
        currentGunso.ShootGun(spawnPoint, currentGunso.bulletSpeed);

        if (currentGunso.currentClipSize <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
            Debug.Log("Clip empty! Reloading...");
        }
    }

    IEnumerator BurstShoot()
    {
        isBurstShooting = true;
        int shotsFired = 0;

        while (shotsFired < currentGunso.burstCount && currentGunso.currentClipSize > 0)
        {
            if (currentGunso.cooldownTimer <= 0f)
            {
                TryShoot();
                shotsFired++;
            }
            yield return null;
        }

        currentGunso.cooldownTimer = currentGunso.shootCooldown;
        isBurstShooting = false;
        burstShootCoroutine = null;
    }

    IEnumerator AutoShoot()
    {
        isBurstShooting = true;

        while (Input.GetButton("Fire1") && currentGunso.currentClipSize > 0 && !isReloading)
        {
            if (currentGunso.cooldownTimer <= 0f)
            {
                TryShoot();
            }
            yield return null;
        }

        currentGunso.cooldownTimer = currentGunso.shootCooldown;
        isBurstShooting = false;
        autoShootCoroutine = null;
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(currentGunso.BulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.transform.Rotate(0f, 0f, 90f);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = spawnPoint.forward * currentGunso.bulletSpeed;
        }

        Destroy(bullet, 5f);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentGunso.reloadTime);
        currentGunso.currentClipSize = currentGunso.maxClipSize;
        isReloading = false;
        Debug.Log("Reloaded!");
    }

    private void StopShootingCoroutines()
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
