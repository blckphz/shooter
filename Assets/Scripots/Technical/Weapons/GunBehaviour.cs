using UnityEngine;
using System.Collections;

public class GunBehaviour : MonoBehaviour
{
    public GunSO[] guns;
    public int currentGunIndex = 0;
    public GunSO currentGunso;
    public Transform spawnPoint;

    [Header("Scale Settings")]
    public float gunXScale = 1f;
    public float gun2XScale = 2f;
    public float gun3YScale = 1.5f;

    [Header("Materials for guns (4 slots)")]
    public Material[] gunMaterials = new Material[4];

    [Header("Gun Renderer (Assign if static gun model)")]
    public MeshRenderer gunRenderer;

    public GameObject extraObject;
    private GunReload gunReload;
    private gunShooting gunShooting;

    // Reference to the flame thrower behaviour if the current gun is a flamethrower
    private FlameThrowerBehaviour flameThrowerBehaviour;

    void Start()
    {
        gunReload = GetComponent<GunReload>();
        gunShooting = GetComponent<gunShooting>();

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
        if (currentGunso == null) return;

        // Adjust cooldownTimer
        if (currentGunso.cooldownTimer > 0f)
        {
            float reductionSpeed = 1f;

            // Faster cooldown reduction when airborne
            ColliderGameOver playerGroundCheck = FindObjectOfType<ColliderGameOver>();
            if (playerGroundCheck != null && !playerGroundCheck.isGrounded)
            {
                reductionSpeed += currentGunso.cooldownRedux; // Apply the bonus
            }

            currentGunso.cooldownTimer -= Time.deltaTime * reductionSpeed;
        }

        // Reduce cooldown faster while reloading
        if (currentGunso.IsReloading && currentGunso.cooldownTimer > 0f)
        {
            currentGunso.cooldownTimer = Mathf.Max(0f, currentGunso.cooldownTimer - Time.deltaTime * 2f);
        }

        // Manual reload input
        if (Input.GetKeyDown(KeyCode.R) && !currentGunso.IsReloading && currentGunso.currentClipSize < currentGunso.maxClipSize)
        {
            gunReload.StartReload(currentGunso);
        }

      if (Input.GetButtonDown("Fire1"))
{
    if (flameThrowerBehaviour != null)
    {
        Debug.Log("Fire1 pressed, starting flame.");
        flameThrowerBehaviour.StartFlame();
    }
}

if (Input.GetButtonUp("Fire1"))
{
    gunShooting.StopShootingCoroutines();

    if (flameThrowerBehaviour != null)
    {
        Debug.Log("Fire1 released, stopping flame.");
        flameThrowerBehaviour.StopFlame();
    }
}




        // Always delegate shooting input to gunShooting (including PortalGun)
        gunShooting.HandleShootingInput(currentGunso);
    }

    public void SwitchGun(GunSO newGun)
    {
        if (newGun == null) return;

        gunShooting.StopShootingCoroutines();

        currentGunso = newGun;
        InitializeGun(currentGunso);

        // Link flame behaviour if needed
        if (currentGunso is FlameThrowerSO)
        {
            flameThrowerBehaviour = GetComponentInChildren<FlameThrowerBehaviour>();
            if (flameThrowerBehaviour != null)
            {
                //flameThrowerBehaviour.SetFlameThrowerSO((FlameThrowerSO)currentGunso);
            }
            else
            {
                Debug.LogWarning("[GunBehaviour] No FlameThrowerBehaviour found in children!");
            }
        }
        else
        {
            flameThrowerBehaviour = null;  // No flame thrower for this gun
        }

        ApplyGunMaterial(currentGunIndex);
        ApplyGunScale(currentGunIndex);

        Debug.Log($"Switched to gun: {currentGunso.name}");
    }

    private void InitializeGun(GunSO gun)
    {
        // Do NOT reset ammo on weapon switch.
        // Only do setup tasks like setting reload timers or resetting states if needed.
        gun.IsReloading = false;
        gun.reloadTimer = 0f;
    }

    private void ApplyGunMaterial(int gunIndex)
    {
        if (gunRenderer == null)
        {
            Debug.LogWarning("Gun Renderer is not assigned in the inspector!");
            return;
        }

        if (gunIndex >= 0 && gunIndex < gunMaterials.Length && gunMaterials[gunIndex] != null)
        {
            gunRenderer.material = gunMaterials[gunIndex];
        }
        else
        {
            Debug.LogWarning($"No material assigned for gun index {gunIndex}");
        }
    }

    private void ApplyGunScale(int gunIndex)
    {
        if (gunRenderer == null) return;

        Vector3 baseScale = Vector3.one;

        switch (gunIndex)
        {
            case 0:
                baseScale = new Vector3(gunXScale, 1f, 1f);
                break;
            case 1:
                baseScale = new Vector3(gun2XScale, 1f, 1f);
                break;
            case 2:
                baseScale = new Vector3(1f, gun3YScale, 1f);
                break;
            default:
                baseScale = Vector3.one;
                break;
        }

        gunRenderer.transform.localScale = baseScale;
    }
}
