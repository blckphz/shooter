using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/Pistol")]
public class Pistol : GunSO
{

    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {

        GameObject bullet = Instantiate(BulletPrefab, spawnPoint.position, spawnPoint.rotation);

        Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * 0.5f; // Offset forward

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = spawnPoint.forward * bulletSpeed; // Launch hook
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
