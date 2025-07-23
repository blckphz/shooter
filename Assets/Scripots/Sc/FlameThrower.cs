
using UnityEngine;

[CreateAssetMenu(fileName = "NewFlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrowerSO : GunSO
{
    [Header("Flamethrower Stats")]
    public float ammoDrainRate = 10f;      // Ammo consumed per second
    public float damagePerSecond = 15f;    // Damage applied per second
    public float x, y, z;
    public AudioClip stioFlame;

    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
{
    // Instantiate flame projectile
    GameObject bullet = Instantiate(BulletPrefab, spawnPoint.position, spawnPoint.rotation);

    // Add random spread
    Vector3 randomSpread = new Vector3(
        Random.Range(-x, x),  // Random horizontal spread
        Random.Range(-y, y),  // Random vertical spread
        0f
    );

    // Apply spread to bullet direction
    Vector3 direction = spawnPoint.forward + spawnPoint.TransformDirection(randomSpread);
    direction.Normalize(); // Normalize so bullet speed is consistent

    // Add velocity to bullet
    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = direction * bulletSpeed;
    }

    // Destroy bullet after a short duration (like flame)
    Destroy(bullet, 2f);
}



}