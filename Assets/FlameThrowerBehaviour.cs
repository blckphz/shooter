using UnityEngine;

public class FlameThrowerBehaviour : MonoBehaviour
{
    public FlameThrowerSO flameThrowerSO;
    private GunBehaviour gunbehav;
    public AudioSource flameLoopSource;
    public AudioSource oneShotSource;

    private bool isPlaying = false;


    private void Awake()
    {
        gunbehav = GetComponentInParent<GunBehaviour>(); // ✅ Correct assignment
    }

    void Update()
    {
        if (flameThrowerSO == null) return;

        if (Input.GetButtonDown("Fire1"))
        {
            StartFlame();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopFlame();
        }

        if (isPlaying)
        {
            flameThrowerSO.UpdateAmmoDrain(Time.deltaTime);

            if (flameThrowerSO.currentClipSize <= 0)
            {
                flameThrowerSO.currentClipSize = 0;
                StopFlame();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

        enemyStats enemy = other.GetComponent<enemyStats>();
            enemy.TakeDamage(flameThrowerSO.damage);
    }



    public void StartFlame()
    {

        if (isPlaying) return;

        if (flameThrowerSO.soundFx == null)
        {
            Debug.LogWarning("No flame loop sound assigned!");
            return;
        }

        if (flameThrowerSO.currentClipSize <= 0)
        {
            Debug.Log("No ammo to start flame.");
            return;
        }

        if (gunbehav.currentGunso == gunbehav.guns[2])
        {
            flameLoopSource.clip = flameThrowerSO.soundFx;
            flameLoopSource.loop = true;
            flameLoopSource.Play();
            isPlaying = true;

        }
    }

    public void StopFlame()
    {
        if (!isPlaying) return;

        if (flameLoopSource.isPlaying)
        {
            flameLoopSource.Stop();
            flameLoopSource.clip = null;
        }

        isPlaying = false;

        if (flameThrowerSO.stioFlame != null)
        {
            oneShotSource.PlayOneShot(flameThrowerSO.stioFlame);
        }
    }
}
