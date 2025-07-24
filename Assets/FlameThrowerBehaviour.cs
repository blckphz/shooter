using UnityEngine;

public class FlameThrowerBehaviour : MonoBehaviour
{
    public FlameThrowerSO flameThrowerSO;

    public AudioSource flameLoopSource;
    public AudioSource oneShotSource;

    private bool isPlaying = false;

    void Update()
    {
        if (flameThrowerSO == null) return;

        // Start flame on input
        if (Input.GetButtonDown("Fire1"))
        {
            StartFlame();
        }
        // Stop flame on input release
        else if (Input.GetButtonUp("Fire1"))
        {
            StopFlame();
        }

        // Auto-stop flame when out of ammo
        if (isPlaying)
        {
            if (flameThrowerSO.currentClipSize <= 0)
            {
                flameThrowerSO.currentClipSize = 0;
                StopFlame();
            }
        }
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

        flameLoopSource.clip = flameThrowerSO.soundFx;
        flameLoopSource.loop = true;
        flameLoopSource.Play();
        isPlaying = true;
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
