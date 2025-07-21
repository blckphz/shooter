using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/Pistol")]
public class Pistol : GunSO
{
    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        // Simple Debug

        // The actual bullet spawning logic could be here or in GunBehaviour.
        // We keep it in GunBehaviour for now.
    }
}
