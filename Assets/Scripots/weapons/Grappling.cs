using UnityEngine;

[CreateAssetMenu(fileName = "NewGrapplingGun", menuName = "Guns/Grappling")]
public class Grappling : GunSO
{
    public GameObject grapplingHookPrefab;

    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        GameObject hook = Instantiate(grapplingHookPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = hook.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = spawnPoint.forward * bulletSpeed; // Launch hook
        }

        grapplingHook hookScript = hook.GetComponent<grapplingHook>();
        if (hookScript != null)
        {
            hookScript.player = spawnPoint.root; // Assign player
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
