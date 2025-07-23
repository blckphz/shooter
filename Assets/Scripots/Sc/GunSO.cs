using UnityEngine;

public enum FireMode
{
    SemiAuto,
    Auto,
    Burst
}

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapons/Gun")]
public abstract class GunSO : ScriptableObject
{
    // -------------------- BASIC SETTINGS --------------------
    [Header("Basic Settings")]
    [Tooltip("Prefab of the bullet to be spawned when shooting.")]
    public GameObject BulletPrefab;

    [Tooltip("Damage dealt by each bullet.")]
    public int damage;

    [Tooltip("Speed of the fired bullet.")]
    public float bulletSpeed;

    [Tooltip("Type of firing mode (SemiAuto, Auto, Burst).")]
    public FireMode fireMode = FireMode.SemiAuto;


    // -------------------- AMMO & RELOADING --------------------
    [Header("Ammo & Reloading")]
    [Tooltip("Maximum number of bullets in a single clip.")]
    public int maxClipSize;

    [HideInInspector]
    public int currentClipSize;

    [Tooltip("Total reserve ammo available.")]
    public int currentReserveAmmo;

    [Tooltip("Time taken to reload the weapon.")]
    public float reloadTime;

    [Tooltip("Is the gun currently reloading?")]
    public bool IsReloading;

    [Tooltip("Reload timer countdown.")]
    public float reloadTimer = 0f;


    // -------------------- FIRING BEHAVIOR --------------------
    [Header("Firing Behavior")]
    [Tooltip("Time between shots in auto or burst mode.")]
    public float ShootFreq;

    [Tooltip("Cooldown reduction factor, if applicable.")]
    public float cooldownRedux;

    [Tooltip("Cooldown time between shots or bursts.")]
    public float shootCooldown;

    [HideInInspector]
    public float cooldownTimer;

    [Tooltip("Number of bullets fired per burst (if in Burst mode).")]
    public int burstCount = 3;


    // -------------------- AUDIO --------------------
    [Header("Audio Clips")]
    [Tooltip("Sound effect played when the gun fires.")]
    public AudioClip soundFx;

    [Tooltip("Sound played when the reload is complete.")]
    public AudioClip reloadCompleteSound;


    // -------------------- ABSTRACT METHODS --------------------
    [Tooltip("Abstract method for shooting (to be implemented in derived classes).")]
    public abstract void ShootGun(Transform spawnPoint, float bulletSpeed);
}
