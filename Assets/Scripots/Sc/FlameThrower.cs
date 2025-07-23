using UnityEngine;

[CreateAssetMenu(fileName = "NewFlameThrower", menuName = "Guns/FlameThrower")]
public class FlameThrowerSO : GunSO
{
    [Header("Flamethrower Stats")]
    public float ammoDrainRate = 10f;      // Ammo consumed per second
    public float damagePerSecond = 15f;    // Damage applied per second

    private FlameThrowerBehaviour flameBehaviour;

    public void SetFlameBehaviour(FlameThrowerBehaviour behaviour)
    {
        flameBehaviour = behaviour;
        Debug.Log("[FlameThrowerSO] Flame behaviour set: " + (behaviour != null));
    }

    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        if (flameBehaviour == null)
        {
            Debug.LogWarning("[FlameThrowerSO] No FlameThrowerBehaviour linked!");
            return;
        }

        if (currentClipSize <= 0)
        {
            Debug.Log("[FlameThrowerSO] No ammo left!");
            flameBehaviour.StopFlame();
            return;
        }

        currentClipSize -= Mathf.CeilToInt(ammoDrainRate * Time.deltaTime);
        if (currentClipSize < 0) currentClipSize = 0;

        Debug.Log($"[FlameThrowerSO] Shooting flame! Ammo left: {currentClipSize}");
        flameBehaviour.PlayFlame();
    }

    public void StopFlame()
    {
        if (flameBehaviour != null)
        {
            flameBehaviour.StopFlame();
            Debug.Log("[FlameThrowerSO] Flame stopped.");
        }
    }
}
