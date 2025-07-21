using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/stickyjump")]

public class JumpMine : GunSO
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        // Simple Debug
        Debug.Log("Pistol shoot!");

        // The actual bullet spawning logic could be here or in GunBehaviour.
        // We keep it in GunBehaviour for now.
    }

    // Update is called once per frame
}
