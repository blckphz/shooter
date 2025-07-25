using UnityEngine;
using System.Collections;

public class gunShooting : MonoBehaviour
{
    private Coroutine autoShootCoroutine;
    private Coroutine burstShootCoroutine;
    private bool isBurstShooting = false;

    private GunReload gunReload;
    private GunBehaviour gunBehaviour;

    private AudioSource audioSource;
    private screenShake screenShakeEffect;
    private ColliderGameOver playerGroundCheck;

    public AudioSource audioSourceShotgun;

    void Start()
    {
        gunReload = GetComponent<GunReload>();
        gunBehaviour = GetComponent<GunBehaviour>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component missing on gun GameObject!");
        }

        screenShakeEffect = FindObjectOfType<screenShake>();
        if (screenShakeEffect == null)
        {
            Debug.LogWarning("No screenShake component found in the scene!");
        }

        playerGroundCheck = FindObjectOfType<ColliderGameOver>();
        if (playerGroundCheck == null)
        {
            Debug.LogWarning("No ColliderGameOver component found in the scene!");
        }
    }

    public void HandleShootingInput(GunSO currentGunso)
    {
        if (currentGunso == null) return;

        if (currentGunso is PortalGun portalGun)
        {
            if (Input.GetButtonDown("Fire1") && !currentGunso.IsReloading)
            {
                TryShootPortal(portalGun, true);
            }
            else if (Input.GetButtonDown("Fire2") && !currentGunso.IsReloading)
            {
                TryShootPortal(portalGun, false);
            }
            return;
        }

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

    private void TryShootPortal(PortalGun portalGun, bool isBlue)
    {
        if (portalGun.IsReloading)
        {
            Debug.Log("Cannot shoot, reloading in progress...");
            return;
        }

        if (portalGun.cooldownTimer > 0f) return;

        if (portalGun.currentClipSize <= 0)
        {
            gunReload.StartReload(portalGun);
            Debug.Log("Clip empty! Reloading...");
            return;
        }

        portalGun.currentClipSize--;

        if (audioSource != null && portalGun.soundFx != null)
        {
            audioSource.PlayOneShot(portalGun.soundFx);
        }

        if (isBlue)
            portalGun.ShootBluePortal(gunBehaviour.spawnPoint, portalGun.bulletSpeed);
        else
            portalGun.ShootOrangePortal(gunBehaviour.spawnPoint, portalGun.bulletSpeed);

        if (screenShakeEffect != null)
        {
            screenShakeEffect.Shake(0.7f, 0.15f);
        }

        float cooldown = portalGun.shootCooldown;
        if (playerGroundCheck != null && !playerGroundCheck.isGrounded)
        {
            cooldown *= (1f - portalGun.cooldownRedux);
        }
        portalGun.cooldownTimer = cooldown;

        if (portalGun.currentClipSize <= 0 && !portalGun.IsReloading)
        {
            gunReload.StartReload(portalGun);
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

        currentGunso.ShootGun(gunBehaviour.spawnPoint, currentGunso.bulletSpeed);


        //REDUES CLIP 

        if (!(currentGunso is FlameThrowerSO))
        {
            currentGunso.currentClipSize--;
        }




        // ONLY play sound if not flame thrower
        if (!(currentGunso is FlameThrowerSO))
        {
            currentGunso.playSound(audioSource);
            Debug.Log(audioSource);
        }

        if (screenShakeEffect != null)
        {
            float shakeStrength = 1.0f;
            screenShakeEffect.Shake(shakeStrength, 0.2f);
        }

        if (currentGunso.fireMode == FireMode.SemiAuto)
        {
            float cooldown = currentGunso.shootCooldown;
            if (playerGroundCheck != null && !playerGroundCheck.isGrounded)
            {
                cooldown *= (1f - currentGunso.cooldownRedux);
            }
            currentGunso.cooldownTimer = cooldown;
        }

        if (currentGunso.currentClipSize <= 0 && !currentGunso.IsReloading)
        {
            gunReload.StartReload(currentGunso);
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

            float cooldown = 1f / currentGunso.ShootFreq;
            if (playerGroundCheck != null && !playerGroundCheck.isGrounded)
            {
                cooldown *= (1f - currentGunso.cooldownRedux);
            }
            currentGunso.cooldownTimer = cooldown;

            yield return null;
        }

        float burstCooldown = currentGunso.shootCooldown;
        if (playerGroundCheck != null && !playerGroundCheck.isGrounded)
        {
            burstCooldown *= (1f - currentGunso.cooldownRedux);
        }
        currentGunso.cooldownTimer = burstCooldown;

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

                float cooldown = 1f / currentGunso.ShootFreq;
                if (playerGroundCheck != null && !playerGroundCheck.isGrounded)
                {
                    cooldown *= (1f - currentGunso.cooldownRedux);
                }
                currentGunso.cooldownTimer = cooldown;
            }
            yield return null;
        }

        float autoCooldown = currentGunso.shootCooldown;
        if (playerGroundCheck != null && !playerGroundCheck.isGrounded)
        {
            autoCooldown *= (1f - currentGunso.cooldownRedux);
        }
        currentGunso.cooldownTimer = autoCooldown;

        isBurstShooting = false;
        autoShootCoroutine = null;
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
