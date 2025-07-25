using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/stickyjump")]

public class JumpMine : GunSO
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        // Simple Debug
        Debug.Log("Pistol shoot!");

        GameObject bullet = Instantiate(BulletPrefab, spawnPoint.position, spawnPoint.rotation);

        Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * 0.5f; // Offset forward



        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = spawnPoint.forward * bulletSpeed; // Launch hook


        // The actual bullet spawning logic could be here or in GunBehaviour.
        // We keep it in GunBehaviour for now.
    }

    public override void playSound(AudioSource audioSource)
    {
        if (audioSource != null && soundFx != null)
        {
            audioSource.PlayOneShot(soundFx);
        }
    }



    // Update is called once per frame
}
