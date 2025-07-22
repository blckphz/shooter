using UnityEngine;

public enum FireMode
{
    SemiAuto,
    Auto,
    Burst
}

public abstract class GunSO : ScriptableObject
{
    public GameObject BulletPrefab;
    public float reloadTime;
    public int maxClipSize;
    public int currentReserveAmmo;
    public float bulletSpeed;
    public float ShootFreq; // for burst or auto-fire
    public bool IsReloading;
    public float reloadTimer = 0f;
    public float cooldownRedux;

    [HideInInspector]
    public int currentClipSize;

    public float shootCooldown; // cooldown between shots / bursts
    [HideInInspector]
    public float cooldownTimer;

    public FireMode fireMode = FireMode.SemiAuto;

    // Burst mode specific
    public int burstCount = 3;         // Number of shots per burst


    public AudioClip soundFx;
    public AudioClip reloadCompleteSound;

    // Abstract method for shooting (implement specific logic in derived classes)
    public abstract void ShootGun(Transform spawnPoint, float bulletSpeed);
}
