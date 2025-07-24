using UnityEngine;

[CreateAssetMenu(fileName = "newShotGun", menuName = "Guns/ShotGun")]
public class shotGun : GunSO
{
    [Header("Shotgun Spread")]
    public float x = 0.1f;
    public float y = 0.1f;
    public float z = 0f;

    [Header("Shotgun Settings")]
    public int pelletsPerShot = 6;   // Number of bullets fired per shot
    public float bulletLifetime = 2f;

    public AudioClip leverreloadsound;

    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        for (int i = 0; i < pelletsPerShot; i++)
        {
            Vector3 randomSpread = new Vector3(
                Random.Range(-x, x),
                Random.Range(-y, y),
                Random.Range(-z, z)
            );

            Vector3 direction = spawnPoint.forward + spawnPoint.TransformDirection(randomSpread);
            direction.Normalize();

            Quaternion bulletRotation = Quaternion.LookRotation(direction);
            GameObject bullet = Instantiate(BulletPrefab, spawnPoint.position, bulletRotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;  // Use velocity, not linearVelocity
            }

            Destroy(bullet, bulletLifetime);
        }

        // Delegate lever sound playback to the gunShooting MonoBehaviour
        gunShooting gunShootComponent = GameObject.FindObjectOfType<gunShooting>();
        if (gunShootComponent != null)
        {
            //gunShootComponent.StartCoroutine(gunShootComponent.PlayLeverSoundDelayed(leverreloadsound, 0.2f));
        }
    }

    public override void playSound(AudioSource audioSource)
    {
        if (audioSource != null && soundFx != null)
        {
            audioSource.PlayOneShot(soundFx);
        }
    }



}
