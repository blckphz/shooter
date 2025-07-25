using UnityEngine;

[CreateAssetMenu(fileName = "NewFlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrowerSO : GunSO
{
    [Header("Flamethrower Stats")]
    public int ammoDrainRate = 10;
    public int damagePerSecond;
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


    private float ammoDrainAccumulator = 0f;

    public void UpdateAmmoDrain(float deltaTime)
    {
        if (currentClipSize <= 0 || ammoDrainRate <= 0) return;

        // Accumulate fractional ammo drain
        ammoDrainAccumulator += ammoDrainRate * deltaTime;

        // Only drain when enough has accumulated for a full unit
        if (ammoDrainAccumulator >= 1f)
        {
            int ammoToDrain = Mathf.FloorToInt(ammoDrainAccumulator);
            int actualDrain = Mathf.Min(currentClipSize, ammoToDrain);

            currentClipSize -= actualDrain;
            ammoDrainAccumulator -= actualDrain;
        }
    }




    public override void playSound(AudioSource audioSource)
    {
        if (audioSource != null && soundFx != null)
        {
            audioSource.PlayOneShot(soundFx);
        }
    }

    /*

    public override void Reload()
    {
    }

    */

}
