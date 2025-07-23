using UnityEngine;

public class FlameThrowerBehaviour : MonoBehaviour
{
    [Header("Flame Particle System (child)")]
    public ParticleSystem flameParticles;

    private void Awake()
    {
        if (flameParticles == null)
        {
            flameParticles = GetComponent<ParticleSystem>();
        }

        if (flameParticles == null)
            Debug.LogError("[FlameThrowerBehaviour] No ParticleSystem found!");
        else
            Debug.Log("[FlameThrowerBehaviour] ParticleSystem ready.");
    }

    public void PlayFlame()
    {
        if (flameParticles != null && !flameParticles.isPlaying)
        {
            flameParticles.Play();
            Debug.Log("[FlameThrowerBehaviour] Flame started.");
        }
    }

    public void StopFlame()
    {
        if (flameParticles != null && flameParticles.isPlaying)
        {
            flameParticles.Stop();
            Debug.Log("[FlameThrowerBehaviour] Flame stopped.");
        }
    }
}
