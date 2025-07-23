using UnityEngine;

public class FlameThrowerBehaviour : MonoBehaviour
{
    public FlameThrowerSO flameThrowerSO;

    private AudioSource flameLoopSource;  // Looping flame sound
    private AudioSource oneShotSource;    // One-shot stop sound

    public bool isPlaying = false;        // Current state flag

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length < 2)
        {
            flameLoopSource = gameObject.AddComponent<AudioSource>();
            oneShotSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            flameLoopSource = sources[0];
            oneShotSource = sources[1];
        }

        // Setup
        flameLoopSource.playOnAwake = false;
        flameLoopSource.loop = true;
        flameLoopSource.volume = 1f;

        oneShotSource.playOnAwake = false;
        oneShotSource.loop = false;
        oneShotSource.volume = 1f;
    }

    public void StartFlame()
    {
        if (isPlaying || flameThrowerSO == null)
        {
            Debug.Log("StartFlame called but already playing or no SO.");
            return;
        }

        isPlaying = true;
        Debug.Log("Flame started!");

        if (flameThrowerSO.soundFx != null)
        {
            flameLoopSource.clip = flameThrowerSO.soundFx;
            flameLoopSource.volume = 1f;
            flameLoopSource.Play();
        }
    }


    public void StopFlame()
    {
        if (!isPlaying)
        {
            Debug.Log("StopFlame called but flame was not playing.");
            return;
        }

        isPlaying = false;

        if (flameLoopSource.isPlaying)
        {
            flameLoopSource.Stop();
        }

        flameLoopSource.clip = null;

        if (flameThrowerSO.stioFlame != null)
        {
            oneShotSource.PlayOneShot(flameThrowerSO.stioFlame);
        }

        Debug.Log("Flamethrower sound stopped.");
    }



    // Optional failsafe (if sound gets stuck due to missed input release)
    private void LateUpdate()
    {
        if (!Input.GetButton("Fire1") && isPlaying)
        {
            StopFlame();  // Enforce stop
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ApplyFlameDamage(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            ApplyFlameDamage(other.gameObject);
        }
    }

    private void ApplyFlameDamage(GameObject enemy)
    {
        var health = enemy.GetComponent<enemyStats>();
        if (health != null)
        {
            health.TakeDamage(flameThrowerSO.damage);
        }
    }
}
