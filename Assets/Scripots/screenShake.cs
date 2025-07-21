using UnityEngine;
using Unity.Cinemachine;

public class screenShake : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        // Add or find an impulse source component
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null)
        {
            impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
        }
    }

    /// <summary>
    /// Call this to trigger screen shake.
    /// </summary>
    /// <param name="amplitude">The strength of the shake</param>
    /// <param name="duration">The duration of the shake</param>
    public void Shake(float amplitude = 1.0f, float duration = 0.2f)
    {
        impulseSource.GenerateImpulse(amplitude);
        // Duration is controlled in the Impulse Listener (on the camera) with decay rate
    }
}
