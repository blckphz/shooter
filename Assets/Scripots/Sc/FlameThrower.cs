using UnityEngine;

[CreateAssetMenu(fileName = "NewFlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrowerSO : GunSO
{
    [Header("Flamethrower Stats")]
    public float ammoDrainRate = 10f;
    public float damagePerSecond = 15f;
    public float x, y, z;
    public AudioClip stioFlame;


    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        GameObject bullet = Instantiate(BulletPrefab, spawnPoint.position, spawnPoint.rotation);

        Vector3 randomSpread = new Vector3(
            Random.Range(-x, x),
            Random.Range(-y, y),
            0f
        );

        Vector3 direction = spawnPoint.forward + spawnPoint.TransformDirection(randomSpread);
        direction.Normalize();

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        Destroy(bullet, 2f);

        // REMOVE any sound playing here!
    }


    public override void playSound(AudioSource audioSource)
    {
        if (audioSource != null && soundFx != null)
        {
            audioSource.PlayOneShot(soundFx);
        }
    }



}
